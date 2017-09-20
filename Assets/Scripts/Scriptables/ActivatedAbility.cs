using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scriptables {

    [CreateAssetMenu(fileName ="Activatable", menuName = "Mutators/Active")]
    public class ActivatedAbility : ScriptableObject {

        public float ChargeTime; 
        public float ChannelTime;
        [Tooltip("If ChannelTime is non-zero, the number of times the effect should 'manifest' while channeling")]
        public int ManifestCount;
        public float CleanupTime;
        [Tooltip("Is Cleanup Time required only after Channeling starts, Otherwise Cleanup is ALWAYS required")]
        public bool OnlyCleanupChanneling;
        public float CooldownTime;
        public AbilityActivationState CooldownStartsWhen = AbilityActivationState.Channeling;

        public AbstractZone ActivationZone;
        public ValueCollection Effects;

        [UnityTag]
        [Tooltip("List of Tags that this ability will (attempt to) affect")]
        public List<string> CanAffectTags;

        [HideInInspector]
        public Transform Owner;

        [HideInInspector]
        public AbilityActivationState ActivationState = AbilityActivationState.Ready;

        [HideInInspector]
        public float ChargeRemaining;
        [HideInInspector]
        public float ChannelRemaining;
        protected float NextManifestAt;

        [HideInInspector]
        public float CleanupEnds;
        [HideInInspector]
        public float CooldownEnds;

        public GameObject ZoneRendererPrefab;

        protected bool WasTriggerDown;

        //public bool CanActivate { get { return ActivationState == AbilityActivationState.Ready && Time.realtimeSinceStartup >= CooldownEnds; } }
        public bool CanActivate { get { return true; } }
        protected float ManifestDelay {  get { return ChannelTime == 0 ? 0 : ChannelTime / ManifestCount; } }

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
        }

        public void Unbind() {
            if (Owner == null) return;

            Owner = null;
        }


        /// <summary>
        /// Returns True on success
        /// </summary>
        /// <returns></returns>
        public bool OnBegin() {
            if (ActivationState != AbilityActivationState.Ready) return false;  // Strictly speaking this should be a CanActivate check, but we're ignoring the cooldown.

            if ( ChargeTime > 0 ) {
                // This is a 'charged'-type activation
                OnBeginCharge();
                
            } else {
                // This is an 'immediate'-type activation, go straight to channeling
                OnBeginChannel();
            }
            return true;
        }

        protected void OnBeginCharge() {
            ActivationState = AbilityActivationState.Charging;
            ChargeRemaining = ChargeTime;
            WasTriggerDown = false;

            if (CooldownStartsWhen == AbilityActivationState.Charging)
                CooldownEnds = Time.realtimeSinceStartup + CooldownTime;
        }

        protected void OnBeginChannel() {
            ActivationState = AbilityActivationState.Channeling;
            ChannelRemaining = ChannelTime;
            WasTriggerDown = false;

            NextManifestAt = Time.realtimeSinceStartup + ManifestDelay;

            if (NextManifestAt <= Time.realtimeSinceStartup)
                OnManifest();

            if (CooldownStartsWhen == AbilityActivationState.Channeling)
                CooldownEnds = Time.realtimeSinceStartup + CooldownTime;
        }

        protected void OnBeginCleanup() {
            if (ActivationState == AbilityActivationState.Charging && OnlyCleanupChanneling) {
                // Skip Cleanup Timer
                ActivationState = AbilityActivationState.Ready;
                CleanupEnds = Time.realtimeSinceStartup;
            }
            else {
                ActivationState = AbilityActivationState.Cleanup;
                CleanupEnds = Time.realtimeSinceStartup + CleanupTime;
            }

            if (CooldownStartsWhen == AbilityActivationState.Cleanup)
                CooldownEnds = Time.realtimeSinceStartup + CooldownTime;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void OnManifest() {
            if (Owner == null) return;

            //  Do Something

            var origin = Owner;

            // Render Zone
            // FIXME: REmove Zone Render
            var zone = ActivationZone as CuboidZone;
            var go = Instantiate(ZoneRendererPrefab, origin.TransformPoint(zone.Bounds.center), origin.rotation);
            go.transform.localScale = zone.Bounds.extents;

            Destroy(go, 0.2f);


            var hits = ActivationZone.ZoneCast(origin);
            foreach ( var hit in hits ) {
                if (!CanAffectTags.Contains(hit.tag)) continue; // Unrecognised Tag

                var attribs = hit.GetComponent<Gameplay.EntityAttributes>();
                if (attribs == null) continue; // No Attribute set to affect

                //foreach (var effect in Effects._Values)
                //    attribs.ApplyEffect(effect.Type, effect.Derived);

                attribs.RemoveHealth(Effects[ValueType.Damage]);
            }
        }


        /// <summary>
        /// Returns True if continuing
        /// </summary>
        /// <returns></returns>
        public bool OnUpdate(bool triggerState) {
            var triggerReleased = IsTriggerReleased(triggerState);

            switch (ActivationState) {
                case AbilityActivationState.Charging:
                    ChargeRemaining = Mathf.Max(ChargeRemaining - Time.realtimeSinceStartup, 0);

                    if (ChargeRemaining == 0 || triggerReleased)
                        OnBeginChannel();

                    return true;
                case AbilityActivationState.Channeling:
                    ChannelRemaining = Mathf.Max(ChannelRemaining - Time.realtimeSinceStartup, 0);

                    if (NextManifestAt <= Time.realtimeSinceStartup) {
                        OnManifest();
                    }

                    if (ChannelRemaining == 0 || triggerReleased) {
                        OnBeginCleanup();
                        return false;
                    }

                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns True when cleanup sequence is complete
        /// </summary>
        /// <returns></returns>
        public bool OnEnd() {
            switch (ActivationState) {
                case AbilityActivationState.Ready:
                    return true;
                case AbilityActivationState.Charging:
                    OnBeginCleanup();
                    break;
                case AbilityActivationState.Channeling:
                    OnBeginCleanup();
                    break;
                case AbilityActivationState.Cleanup:
                    if (Time.realtimeSinceStartup >= CleanupEnds) {
                        ActivationState = AbilityActivationState.Ready;
                        return true;
                    }
                    break;
                    
            }

            return Time.realtimeSinceStartup >= CleanupEnds;
        }



    }
}