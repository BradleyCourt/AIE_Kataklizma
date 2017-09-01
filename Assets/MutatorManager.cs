using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptables;

namespace Gameplay {
    public class MutatorManager : MonoBehaviour {

        public List<ActivatedAbility> ActiveAbilities = new List<ActivatedAbility>(1);
        public List<PassiveAbility> PassiveAbilities = new List<PassiveAbility>(1);


        private List<ActivatedAbility> RequireUpdate = new List<ActivatedAbility>(1);

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

            for ( var i = 0; i < ActiveAbilities.Count; i++ ) {
                var ability = ActiveAbilities[i];
                var trigger = "Fire" + (i + 1);
                if (ability == null) continue;

                switch (ability.ActivationType) {
                    case AbilityActivationType.Normal:
                        ActivateNormalAbility(ability, trigger);
                        break;
                    case AbilityActivationType.Channeled:
                        ActivateChanneledAbility(ability, trigger);
                        break;
                    case AbilityActivationType.Charged:
                        ActivateChargedAbility(ability, trigger);
                        break;

                }

            }

        }


        int InsertAbility( ActivatedAbility ability, int slot = -1 ) {
            if (slot >= ActiveAbilities.Count)
                throw new System.IndexOutOfRangeException(gameObject.name + " - MutatorManager.InsertAbility(): Slot index out of range: " + slot);

            if ( slot < 0 ) { // If slot is negative, find first available empty slot
                var slotted = false;
                for (slot = 0; slot < ActiveAbilities.Count && !slotted; slot++)
                    slotted = (ActiveAbilities[slot] == null);

                if (!slotted)
                    return -1;
            }

            // At this point, slot must be a valid index

            if (ActiveAbilities[slot] != null)
                ActiveAbilities[slot].DeregisterOwner();

            ActiveAbilities[slot] = (ability == null ? null : Instantiate(ability));

            if (ActiveAbilities[slot] != null) {
                ActiveAbilities[slot].RegisterOwner(transform);
                ActiveAbilities[slot].Reset();
            }


            return slot;
        }


        /// <summary>
        /// "Click-Once Activation"
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="trigger"></param>
        void ActivateNormalAbility( ActivatedAbility ability, string trigger ) {

            // If ability is ready and trigger held
            {
                // Set state to preparing
            }

            // if ability is preparing
            {
                // Tick down timer.
                // If timer is zero, set ability to Active
            }

            // if Ability is active, 
            {
                // perform effects on all in zone

            }
        }


        /// <summary>
        /// "Hold to continue activation"
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="trigger"></param>
        void ActivateChanneledAbility(ActivatedAbility ability, string trigger) { }


        /// <summary>
        /// "Hold to charge up, release to activate"
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="trigger"></param>
        void ActivateChargedAbility(ActivatedAbility ability, string trigger) { }


        #region " Action: Melee Attack "

        public BoxCollider MeleeAttackZone;
        public float MeleeAttackCooldown = 1;
        public float MeleeAttackDuration = 0.3f;
        public int MeleeAttackDamage = 10;
        private bool IsMeleeAttackReady = true;
        private bool CanMeleeAttack {
            get {
                return true; // IsMeleeAttackReady && State == AnimState.Idle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void DoMeleeAttack() {

            //State = AnimState.Attacking;

            // Setup delay for returning control "when it stops"
            StartCoroutine(this.DelayedAction(MeleeAttackDuration,
                () => {
                //if (State == AnimState.Attacking) {
                //    State = AnimState.Idle;
                //}
            }));

            // Setup delay for cooldown "when it can be done again"
            StartCoroutine(this.DelayedAction(MeleeAttackCooldown,
                () => {
                    IsMeleeAttackReady = true;
                }));


            //// Render Zone
            //// FIXME: REmove Zone Render
            //var go = Instantiate(MeleeAttackZoneRenderer.gameObject, MeleeAttackZoneRenderer.transform.parent, false);
            //go.transform.parent = null;
            //go.GetComponent<MeshRenderer>().enabled = true;
            //Destroy(go, MeleeAttackDuration);

            // Do Attack
            var centreOffset = transform.localToWorldMatrix.MultiplyVector(MeleeAttackZone.center);

            var centre = transform.position + centreOffset;
            var size = MeleeAttackZone.size; // transform.localToWorldMatrix.MultiplyVector(MeleeAttackZone.size);

            Collider[] targets = Physics.OverlapBox(centre, size, transform.rotation, ~8); // Anything that is NOT a PlayerAvatar layer

            foreach (Collider target in targets) {
                if (target.gameObject.tag == "Player") continue;


                if (target.gameObject.tag == "Enemy" || target.gameObject.tag == "Building") {
                    var playerStats = target.GetComponent<EntityAttributes>();
                    if (playerStats) {
                        playerStats.RemoveHealth(MeleeAttackDamage);
                    }
                }
            }
        }
        #endregion
    }
}