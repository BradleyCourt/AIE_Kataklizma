using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "PrefabEffect", menuName = "Kataklizma/Effects/Particle Effect")]
    public class PrefabEffect : AnchoredEffect {

        public GameObject Prefab;
        public bool RequiresLocalScale;

        protected GameObject Instance;

        // TODO:  Fix Error Messages

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void Apply(Params data) {
            base.Apply(data);

            if (OwnerOrigins == null) {
                Debug.LogError(Owner.gameObject.name + " - PrefabEffect::Apply(): Prefab Effect requires child with CharacterBindOrigins component");
            }
            else {
                if (OwnerOrigins[Anchor] == null) {
                    Debug.LogError(Owner.gameObject.name + " - PrefabEffect::Apply(): Character Bind Origin [" + Anchor.ToString() + "] is not set");
                }
                else {
                    if ( Instance != null && Instance ) {
                        Debug.LogError(Owner.gameObject.name + " - PrefabEffect::Apply(): Tried to apply but instance already active");
                    }

                    Instance = Instantiate(Prefab, OwnerOrigins[Anchor].position, Owner.rotation, OwnerOrigins[Anchor]);

                    if (RequiresLocalScale)
                        Instance.transform.localScale = Owner.transform.localScale;

                    if (!float.IsInfinity(data.Duration))
                        Destroy(Instance, data.Duration);
                }
            }
        }

        public override void Remove() {
            base.Remove();

            if (Instance != null && Instance) {
                Destroy(Instance);
            }
        }
    }
}
