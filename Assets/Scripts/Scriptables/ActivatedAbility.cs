using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scriptables {

    [CreateAssetMenu(fileName ="Activatable", menuName = "Mutators/Active")]
    public class ActivatedAbility : ScriptableObject {



        public enum ActivationState {
            Ready,
            Charging,
            Preparing,
            Active,
            Cooldown
        }

        [Tooltip("Style of Activation")]
        public AbilityActivationType ActivationType = AbilityActivationType.Normal;

        [Tooltip("Number of stored activations")]
        public float Activations = 1;

        [Tooltip("Time before an Activation regenerates")]
        public float Cooldown = 1;

        [Tooltip("Time taken to perform Ability")]
        public float Duration = 1; 

        [Tooltip("Time to \"charge up\" ability before it's activated")]
        public float Precharge = 1;

        [Tooltip("Dead time between Precharge and Activation")]
        public float Delay = 0;

        [Tooltip("Area of Effect")]
        public AbstractZone ActivationZone;


        public ActivationState State { get; protected set; }


        public float ActivationsRemaining { get; set; }
        public List<float> _CooldownRemaining;
        public List<float> CooldownRemaining { get; set; }

        [HideInInspector]
        public float DurationRemaining;
        [HideInInspector]
        public float PrechargeRemaining;
        [HideInInspector]
        public float DelayRemaining;

        public void Reset() { State = ActivationState.Ready; }
        public void IncrementState() { }
        public void BeginCooldown() { }


        public bool IsReady {  get { return State == ActivationState.Ready; } }

        public ValueCollection Effects;


        protected Transform Owner;
        public void RegisterOwner( Transform owner ) {
            Owner = owner;
        }

        public void DeregisterOwner() { }


        public bool CanUpdate { get { return false; } }
        public void Update() {

            CooldownRemaining = Mathf.Max(CooldownRemaining - Time.deltaTime, 0);
            DurationRemaining = Mathf.Max(DurationRemaining - Time.deltaTime, 0);
            PrechargeRemaining = Mathf.Max(PrechargeRemaining - Time.deltaTime, 0);
            DelayRemaining = Mathf.Max(DelayRemaining - Time.deltaTime, 0);

        }
    }
}