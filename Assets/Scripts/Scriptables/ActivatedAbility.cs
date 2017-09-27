using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scriptables {

    [CreateAssetMenu(fileName ="Activatable", menuName = "Mutators/Active")]
    public class ActivatedAbility : ScriptableObject {

        [System.Serializable]
        public struct AnimationOptions {
            public string OnCharging;
            public string OnChannelling;
            public string OnCleanup;
            public string OnManifest;
        }

        [Header("Timers")]
        public float ChargeTime; 
        public float ChannelTime;

        [Tooltip("If ChannelTime is non-zero, the number of times the effect should 'manifest' while channeling")]
        public int ManifestCount;
        public float CleanupTime;
        public bool ChargeCausesCleanup;
        public float CooldownTime;

        [Tooltip("Setting \"Cooldown Starts When Ready\" will disable cooldowns")]
        public AbilityActivationState CooldownStartsWhen = AbilityActivationState.Channeling;

        [Header("Gameplay Effects")]
        public AbstractZone ActivationZone;
        public CharacterBindPoint ActivationAnchor;

        public List<ValueCollection.Value> Effects;

        [UnityTag]
        [Tooltip("List of Tags that this ability will (attempt to) affect")]
        public List<string> CanAffectTags;

        [Header("UX Effects")]
        public AnimationOptions Animation;
        public ScriptedEffect ParticleEffect;
        

        protected Transform Owner { get; set; }
        protected Animation OwnerAnimator { get; set; }
        protected CharacterBindOrigins OwnerOrigins;

        public AbilityActivationState ActivationState { get; protected set; }

        

        public float ChargeRemaining { get; protected set; }
        public float ChannelRemaining { get; protected set; }
        protected float NextManifestAt;

        public float CleanupEnds { get; protected set; }
        public float CooldownEnds { get; protected set; }
        
        protected bool WasTriggerDown;

        public bool CanActivate { get { return ActivationState == AbilityActivationState.Ready && Time.time >= CooldownEnds; } }
        
        protected float ManifestDelay {  get { return ChannelTime <= 1 ? 0 : ChannelTime / (ManifestCount - 1); } }

        protected bool IsTriggerReleased(bool triggerState) {
            var result = (WasTriggerDown && !triggerState);

            if (triggerState) WasTriggerDown = true; // Latch WasTriggerDown

            return result;
        }


        public ActivatedAbility CloneAndBind(Transform owner) {
            var result = Instantiate(this);
            result.Bind(owner);
            return result;
        }

        public void Bind( Transform owner ) {
            if (owner == null) Unbind();

            Owner = owner;
            OwnerAnimator = Owner.GetComponent<Animation>();
            OwnerOrigins = Owner.GetComponentInChildren<CharacterBindOrigins>();
        }

        public void Unbind() {
            if (Owner == null) return;

            Owner = null;
        }

        public void Reset() {
            ChargeRemaining = ChargeTime;
            ChannelRemaining = ChannelTime;
            CleanupEnds = Time.time;
            CooldownEnds = Time.time;
        }

        /// <summary>
        /// Returns True on success
        /// </summary>
        /// <returns></returns>
        public bool OnBegin() {
            return OnBeginCharge();
        }

        protected bool OnBeginCharge() {
            if (CooldownStartsWhen == AbilityActivationState.Charging)
                CooldownEnds = Time.time + CooldownTime;

            var result = false;

            if (ChargeTime > 0) { // Requires Charging
                ActivationState = AbilityActivationState.Charging;
                ChargeRemaining = ChargeTime;
                WasTriggerDown = false;

                // Do Animation
                if (OwnerAnimator != null && !string.IsNullOrEmpty(Animation.OnCharging))
                    OwnerAnimator.Play(Animation.OnCharging);

                // Do Ux Effects
                // Do Gameplay Effect

                result = true;
            }
            else {
                // Go straight to channeling
                result = OnBeginChannel();
            }
            return result;
        }


        protected bool OnBeginChannel() {
            if (CooldownStartsWhen == AbilityActivationState.Channeling)
                CooldownEnds = Time.time + CooldownTime;

            OnManifest();

            var result = false;
            if (ChannelTime > 0) { // Has Channel Time
                ActivationState = AbilityActivationState.Channeling;
                ChannelRemaining = ChannelTime;
                WasTriggerDown = false;

                // Do Animation
                if (OwnerAnimator != null && !string.IsNullOrEmpty(Animation.OnChannelling))
                    OwnerAnimator.Play(Animation.OnChannelling);

                // Do Ux Effects
                if ( ParticleEffect != null ) {
                    var effect = ParticleEffect as PrefabEffect;
                    var go = Instantiate(effect.Prefab, OwnerOrigins[effect.Location]);
                    Destroy(go, ChannelTime);
                }

                // Do Gameplay Effect

                result = true;
            }
            else { // No Channel Time
                result = OnBeginCleanup();
            }
            return result;
        }

        protected bool OnBeginCleanup() {
            if (CooldownStartsWhen == AbilityActivationState.Cleanup)
                CooldownEnds = Time.time + CooldownTime;

            var result = false;

            // Has a cleanup time and (is channelling or (is charging and charging causes cleanup))
            var requireCleanup = CleanupTime > 0 && (ActivationState == AbilityActivationState.Channeling || (ActivationState == AbilityActivationState.Charging && ChargeCausesCleanup));    

            if (requireCleanup) {
                // Do Cleanup
                ActivationState = AbilityActivationState.Cleanup;
                CleanupEnds = Time.time + CleanupTime;

                // Do Animation
                if (OwnerAnimator != null && !string.IsNullOrEmpty(Animation.OnCleanup))
                    OwnerAnimator.Play(Animation.OnCleanup);

                // Do Ux Effects
                // Do Gameplay Effects

                result = true;
            }
            else {
                // No Cleanup
                ActivationState = AbilityActivationState.Ready;
                CleanupEnds = Time.time;

                result = false;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void OnManifest() {
            NextManifestAt = Time.time + ManifestDelay;

            if (Owner == null) return;

            //  Do Something


            // Do Animation
            if (OwnerAnimator != null && !string.IsNullOrEmpty(Animation.OnManifest))
                OwnerAnimator.Play(Animation.OnManifest);

            // Do Ux Effects
            // Do Gameplay Effects

            var origin = Owner;

            var hits = ActivationZone.OverlapZone(origin);
            foreach (var hit in hits) {
                if (!CanAffectTags.Contains(hit.tag)) continue; // Unrecognised Tag

                var attribs = hit.GetComponent<Gameplay.EntityAttributes>();
                if (attribs == null) continue; // No Attribute set to affect

                foreach (var effect in Effects)
                    attribs.ApplyEffect(effect.Type, effect.Derived);
            }
        }


        /// <summary>
        /// Returns True if continuing
        /// </summary>
        /// <returns></returns>
        public bool OnUpdate(bool triggerState) {
            var triggerReleased = IsTriggerReleased(triggerState);

            switch (ActivationState) {
                case AbilityActivationState.Ready:
                    return false;
                case AbilityActivationState.Charging:
                    ChargeRemaining = Mathf.Max(ChargeRemaining - Time.deltaTime, 0);

                    if (ChargeRemaining == 0 || triggerReleased)
                        OnBeginChannel();

                    return true;
                case AbilityActivationState.Channeling:
                    ChannelRemaining = Mathf.Max(ChannelRemaining - Time.deltaTime, 0);

                    if (ManifestCount > 1 && NextManifestAt <= Time.time) {
                        OnManifest();
                    }

                    if (ChannelRemaining == 0 || triggerReleased) {
                        return OnBeginCleanup();
                    }

                    return true;
                case AbilityActivationState.Cleanup:
                    return Time.time < CleanupEnds;
            }

            return false;
        }

        /// <summary>
        /// Force-End 'cancel' the current activation state
        /// </summary>
        public void OnEnd() {
            switch (ActivationState) {
                case AbilityActivationState.Ready:
                    break;
                case AbilityActivationState.Charging:
                    OnBeginCleanup();
                    break;
                case AbilityActivationState.Channeling:
                    OnBeginCleanup();
                    break;
                case AbilityActivationState.Cleanup:
                    break;
                    
            }
        }



    }
}