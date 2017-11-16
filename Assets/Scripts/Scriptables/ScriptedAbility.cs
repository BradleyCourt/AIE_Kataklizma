using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scriptables {

    [CreateAssetMenu(fileName ="Ability", menuName = "Kataklizma/New Ability")]
    public class ScriptedAbility : ScriptableObject {


        public int weight;
        public Sprite icon;

        [Header("Unlock Progression")]
        public int RequiredLevel;
        public List<ScriptedAbility> Prerequisites;
        public List<ScriptedAbility> Precluders;

        [System.Serializable]
        public struct EffectOptions {

            [Tooltip("Always Active, \"Status Effect\" Types.")]
            public List<ScriptedEffect> Lifetime;
            public List<ScriptedEffect> OnCharging;
            public List<ScriptedEffect> OnChannelling;
            public List<ScriptedEffect> OnCleanup;
            public List<ScriptedEffect> OnManifest;

            public List<ScriptedEffect> AllEffects {
                get {
                    var result = new List<ScriptedEffect>();
                    result.AddRange(Lifetime);
                    result.AddRange(OnCharging);
                    result.AddRange(OnChannelling);
                    result.AddRange(OnCleanup);
                    result.AddRange(OnManifest);

                    return result;
                }
            }
        }

        [Header("Timers")]
        public float ChargeTime; 
        public float ChannelTime;
        public float CleanupTime;

        [Tooltip("If ChannelTime is non-zero, the number of times the effect should 'manifest' while channeling")]
        public int ManifestCount;

        [Space]
        public float CooldownTime;
        
        public AbilityActivationState CooldownStartsAfter = AbilityActivationState.Channeling;

        [Space]
        [Tooltip("PlayerController will immediately re-activate this ability when cooldown ends")]
        public bool ContinuousActivation;

        [Space]
        [Tooltip("Cleanup timer starts after a cancelled charge, else only starts after cancelled or completed channel")]
        public bool ChargeCausesCleanup;
        public bool CanCancelCharge;
        public bool CanCancelChannel;

        [Space]
        public bool LockMovement;
        public AbilityRotationMode LockRotation;      
        
        [Space]
        public CharacterBindPoint AreaAnchor;

        [Tooltip("A Null AoE will result in zero \"others\" but can still affect self")]
        public ScriptedZone AreaOfEffect;

        [Space]
        public EffectOptions Effects;



        protected CharacterBindOrigins _OwnerOrigins;
        protected CharacterBindOrigins OwnerOrigins {
            get {
                if (Owner == null) return null;
                if (_OwnerOrigins == null)
                    _OwnerOrigins = Owner.GetComponentInChildren<CharacterBindOrigins>();

                return _OwnerOrigins;
            }
        }

        protected Transform Owner { get; set; }

        public AbilityActivationState ActivationState { get; protected set; }

        public float DurationTotal {  get { return ChargeTime + ChannelTime + CleanupTime; } }
        public float DurationRemaining {
            get {
                return ChargeRemaining + ChannelRemaining + (ActivationState != AbilityActivationState.Cleanup ? CleanupTime : Mathf.Clamp(CleanupEnds - Time.time, 0, CleanupTime));
            }
        }

        

        public float ChargeRemaining { get; protected set; }
        public float ChannelRemaining { get; protected set; }
        protected float NextManifestAt;

        public float CleanupEnds { get; protected set; }
        public float CooldownEnds { get; protected set; }
        
        protected bool WasTriggerDown;

        public bool CanActivate { get { return ActivationState == AbilityActivationState.Ready && Time.time >= CooldownEnds; } }
        
        protected float ManifestDelay {  get { return ManifestCount <= 1 ? 0 : ChannelTime / ManifestCount; } }

        protected bool IsTriggerReleased(bool triggerState) {
            var result = (WasTriggerDown && !triggerState);

            if (triggerState) WasTriggerDown = true; // Latch WasTriggerDown

            return result;
        }


        public ScriptedAbility CloneAndBind(Transform owner) {
            var result = Instantiate(this);

            result.Owner = owner;

            var maxCount = Mathf.Max(Effects.Lifetime.Count, Effects.OnCharging.Count, Effects.OnChannelling.Count, Effects.OnCleanup.Count, Effects.OnManifest.Count);
            for ( var i = 0; i < maxCount; i++ )
            {
                if (i < result.Effects.Lifetime.Count && result.Effects.Lifetime[i] != null)
                    result.Effects.Lifetime[i] = result.Effects.Lifetime[i].CloneAndBind(owner);

                if (i < Effects.OnCharging.Count && result.Effects.OnCharging[i] != null)
                    result.Effects.OnCharging[i] = result.Effects.OnCharging[i].CloneAndBind(owner);

                if (i < result.Effects.OnChannelling.Count && result.Effects.OnChannelling[i] != null)
                    result.Effects.OnChannelling[i] = result.Effects.OnChannelling[i].CloneAndBind(owner);

                if (i < result.Effects.OnCleanup.Count && result.Effects.OnCleanup[i] != null)
                    result.Effects.OnCleanup[i] = result.Effects.OnCleanup[i].CloneAndBind(owner);

                if (i < result.Effects.OnManifest.Count && result.Effects.OnManifest[i] != null)
                    result.Effects.OnManifest[i] = result.Effects.OnManifest[i].CloneAndBind(owner);
            }

            return result;
        }

        public void Bind( Transform owner ) {
            if (owner == null) Unbind();

            Owner = owner;

            foreach ( var effect in Effects.AllEffects) {
                if (effect == null) continue;
                effect.Bind(Owner);
            }
        }

        public void Unbind() {
            if (Owner == null) return;

            Owner = null;

            foreach (var effect in Effects.AllEffects) {
                if (effect == null) continue;
                effect.Unbind();
            }
        }

        void OnDestroy()
        {
            foreach (var effect in Effects.AllEffects)
            {
                if (effect == null) continue;
                Destroy(effect);
            }
        }

        public void Reset() {
            ActivationState = AbilityActivationState.Ready;
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
            if (CooldownStartsAfter == AbilityActivationState.Ready)
                CooldownEnds = Time.time + CooldownTime;

            var result = false;
            
            if (ChargeTime > 0) { // Has Charge Time
                ActivationState = AbilityActivationState.Charging;
                ChargeRemaining = ChargeTime;
                WasTriggerDown = false; // Reset Trigger latch


                // Do Ux Effects & Gameplay Effects
                ApplyEffects(Effects.OnCharging, ChargeTime);

                result = true;
            }
            else {
                // Go straight to channeling
                result = OnBeginChannel();
            }
            return result;
        }


        protected bool OnBeginChannel() {
            if (CooldownStartsAfter == AbilityActivationState.Charging)
                CooldownEnds = Time.time + CooldownTime;

            // Remove any Charging effects
            RemoveEffects(Effects.OnCharging);

            // Manifest once
            OnManifest();

            var result = false;
            if (ChannelTime > 0) { // Has Channel Time
                ActivationState = AbilityActivationState.Channeling;
                ChannelRemaining = ChannelTime;
                WasTriggerDown = false; // Reset Trigger Latch


                // Do Ux Effects & Gameplay Effects
                ApplyEffects(Effects.OnChannelling, ChannelTime);

                result = true;
            }
            else { // No Channel Time
                result = OnBeginCleanup();
            }
            return result;
        }

        protected bool OnBeginCleanup() {
            if (CooldownStartsAfter == AbilityActivationState.Channeling || CooldownStartsAfter == AbilityActivationState.Cleanup)
                CooldownEnds = Time.time + CooldownTime + (CooldownStartsAfter == AbilityActivationState.Cleanup ? CleanupTime : 0);

            // Remove any Channelling effects
            RemoveEffects(Effects.OnChannelling);

            var result = false;

            // Has a cleanup time and (is channelling or (is charging and charging causes cleanup))
            var requireCleanup = CleanupTime > 0 && (ActivationState == AbilityActivationState.Channeling || (ActivationState == AbilityActivationState.Charging && ChargeCausesCleanup));    

            if (requireCleanup) {
                // Do Cleanup
                ActivationState = AbilityActivationState.Cleanup;
                CleanupEnds = Time.time + CleanupTime;

                // Do Ux & Gameplay Eff
                ApplyEffects(Effects.OnCleanup, CleanupTime);

                result = true;
            }
            else {
                // No Cleanup
                result = OnBeginEnd();
            }
            return result;
        }


        protected bool OnBeginEnd() {
            ActivationState = AbilityActivationState.Ready;
            ChargeRemaining = ChargeTime;
            ChannelRemaining = ChannelTime;

            RemoveEffects(Effects.OnCleanup);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void OnManifest() {
            NextManifestAt = Time.time + ManifestDelay;
            ApplyEffects(Effects.OnManifest, ManifestDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ApplyEffects(List<ScriptedEffect> effects, float timeToLive = float.PositiveInfinity) {
            if (Owner == null) return;
            if (effects == null || effects.Count == 0) return;

            var data = new ScriptedEffect.Params();

            data.Duration = timeToLive;

            // Find Effect Origin and Discover Hits
            if (AreaOfEffect != null) {
                Transform origin = null;

                if (OwnerOrigins != null)
                    origin = OwnerOrigins[AreaAnchor];

                if (origin == null)
                    origin = Owner;

                data.Hits = AreaOfEffect.OverlapZone(Matrix4x4.TRS(origin.position, Owner.rotation, origin.lossyScale));
            }

            foreach (var effect in effects) {
                if (effect == null) continue;
                effect.Apply(data);
            }
        }

        
        protected void RemoveEffects(List<ScriptedEffect> effects) {
            if (Owner == null) return;
            if (effects == null || effects.Count == 0) return;

            foreach (var effect in effects) {
                if (effect == null) continue;
                effect.Remove();
            }
        }

        /// <summary>
        /// Returns True if continuing
        /// </summary>
        /// <returns></returns>
        public bool OnUpdate(bool triggerState) {
            var triggerReleased = IsTriggerReleased(triggerState);

            var result = false;

            switch (ActivationState) {
                case AbilityActivationState.Ready:
                    result = false;
                    break;
                case AbilityActivationState.Charging:
                    ChargeRemaining = Mathf.Max(ChargeRemaining - Time.deltaTime, 0);

                    if (ChargeRemaining == 0 || (CanCancelCharge && triggerReleased))
                        result = OnBeginChannel();
                    else
                        result = true;

                    break;
                case AbilityActivationState.Channeling:
                    ChannelRemaining = Mathf.Max(ChannelRemaining - Time.deltaTime, 0);

                    if (ManifestCount > 1 && NextManifestAt <= Time.time) 
                        OnManifest();
                                        
                    if (ChannelRemaining == 0 || (CanCancelChannel && triggerReleased) )
                        result = OnBeginCleanup();
                    else
                        result = true;

                    break;                    
                case AbilityActivationState.Cleanup:

                    result = Time.time < CleanupEnds;
                    
                    if (!result) { // if Cleanup is finished
                        OnBeginEnd();
                    }

                    break;
            }

            return result;
        }
    }
}