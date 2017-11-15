using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kataklizma.Gameplay {
    public class AbilityUI : MonoBehaviour {


        protected PlayerController Controller;
        protected AbilityWatcher[] Items;


        // Use this for initialization
        void Start() {

            Controller = FindObjectOfType<PlayerController>();

            Items = new AbilityWatcher[0];
        }

        // Update is called once per frame
        void Update() {

            if (Items.Length != Controller.Abilities.Count)
                ResizeItems();

            for (var idx = 0; idx < Items.Length; idx++) {
                if ( Items[idx].TargetAbility != Controller.Abilities[idx].Ability) {
                    ResizeItems();
                    break;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        void ResizeItems() {
            var old = Items;

            Items = new AbilityWatcher[Controller.Abilities.Count];

            for ( var idx = 0; idx < old.Length || idx < Items.Length; idx++) {
                if ( idx < old.Length && idx < Items.Length) {
                    Items[idx] = old[idx];
                    RebindItem(idx);
                }
                else if (idx < old.Length) {
                    Destroy(old[idx].gameObject);
                } else {
                    var go = Instantiate(Resources.Load<GameObject>("AbilityElement"), transform);
                    go.transform.position += new Vector3(70, 0, 0) * idx;
                    go.name = "Ability " + idx;

                    Items[idx] = go.GetComponent<AbilityWatcher>();

                    RebindItem(idx);
                }
            }
        }

        void RebindItem(int idx) {
            
            Items[idx].TargetAbility = Controller.Abilities[idx].Ability;

            if (Controller.Abilities[idx].Ability == null) {
                Items[idx].gameObject.SetActive(false);
            }
            else {
                Items[idx].gameObject.SetActive(true);
                Items[idx].Foreground.sprite = Controller.Abilities[idx].Ability.icon ?? Items[idx].Foreground.sprite;
            }
            
        }
    }
}