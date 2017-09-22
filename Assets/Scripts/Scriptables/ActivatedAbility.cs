﻿using System.Collections;
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

        [Tooltip("Is Cleanup Time required only after Channeling starts, Otherwise Cleanup is ALWAYS required")]
        public bool ChargeCausesCleanup;
        public float CooldownTime;

        public AbilityActivationState CooldownStartsWhen = AbilityActivationState.Channeling;

        [Header("Gameplay Effects")]
        public AbstractZone ActivationZone;
        public ValueCollection Effects;

        [UnityTag]
        [Tooltip("List of Tags that this ability will (attempt to) affect")]
        public List<string> CanAffectTags;

        [Header("UX Effects")]
        public AnimationOptions Animation;

        protected Transform Owner { get; set; }
        protected Animation OwnerAnimator { get; set; }

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
            //if (ActivationState != AbilityActivationState.Ready) return false;  // Strictly speaking this should be a CanActivate check, but we're ignoring the cooldown.

            OnBeginCharge();

            return true;
        }

        protected void OnBeginCharge() {
            if (CooldownStartsWhen == AbilityActivationState.Charging)
                CooldownEnds = Time.time + CooldownTime;

            if (ChargeTime > 0) { // Requires Charging
                ActivationState = AbilityActivationState.Charging;
                ChargeRemaining = ChargeTime;
                WasTriggerDown = false;
            }
            else {
                // Go straight to channeling
                OnBeginChannel();
            }
        }

        protected void OnBeginChannel() {
            if (CooldownStartsWhen == AbilityActivationState.Channeling)
                CooldownEnds = Time.time + CooldownTime;

            OnManifest();

            if (ChannelTime > 0) { // Has Channel Time
                ActivationState = AbilityActivationState.Channeling;
                ChannelRemaining = ChannelTime;
                WasTriggerDown = false;
            }
            else { // No Channel Time
                OnManifest();
                OnBeginCleanup();
            }
        }

        protected void OnBeginCleanup() {
            if (CooldownStartsWhen == AbilityActivationState.Cleanup)
                CooldownEnds = Time.time + CooldownTime;

            // Has a cleanup time and (is channelling or (is charging and charging causes cleanup))
            var requireCleanup = CleanupTime > 0 && (ActivationState == AbilityActivationState.Channeling || (ActivationState == AbilityActivationState.Charging && ChargeCausesCleanup));    

            if (requireCleanup) {
                // Do Cleanup
                ActivationState = AbilityActivationState.Cleanup;
                CleanupEnds = Time.time + CleanupTime;
            }
            else {
                // No Cleanup
                ActivationState = AbilityActivationState.Ready;
                CleanupEnds = Time.time;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void OnManifest() {
            NextManifestAt = Time.time + ManifestDelay;

            if (Owner == null) return;

            //  Do Something

            var origin = Owner;

            var hits = ActivationZone.OverlapZone(origin);
            foreach (var hit in hits) {
                if (!CanAffectTags.Contains(hit.tag)) continue; // Unrecognised Tag

                var attribs = hit.GetComponent<Gameplay.EntityAttributes>();
                if (attribs == null) continue; // No Attribute set to affect

                foreach (var effect in Effects._Values)
                    attribs.ApplyEffect(effect.Type, effect.Derived);

                //attribs.RemoveHealth(Effects[ValueType.Damage]);
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
                        OnBeginCleanup();
                        return false;
                    }

                    return true;
                case AbilityActivationState.Cleanup:
                    return false;
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
                    if (Time.time >= CleanupEnds) {
                        ActivationState = AbilityActivationState.Ready;
                        return true;
                    }
                    break;
                    
            }

            return Time.time >= CleanupEnds;
        }



    }
}