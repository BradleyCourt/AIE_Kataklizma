﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "PrefabEffect", menuName = "Kataklizma/Effects/Prefab Effect")]
    public class PrefabEffect : AnchoredEffect {

        public GameObject Prefab;
        public bool RequiresLocalScale;
        public bool SpawnAsChild = false;

        protected GameObject Instance;

        // TODO:  Fix Error Messages

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void Apply(Params data) {
            if (IsRemovable && IsApplied) return; // Is already applied

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
                        Debug.LogWarning(Owner.gameObject.name + " - PrefabEffect::Apply(): Tried to apply but instance already active");
                        return;
                    }

                    Instance = Instantiate(Prefab, OwnerOrigins[Anchor].position, Owner.rotation, OwnerOrigins[Anchor]);

                    if (RequiresLocalScale)
                        Instance.transform.localScale = Owner.transform.localScale;

                    if (SpawnAsChild)
                        Instance.transform.parent = Owner.transform;

                    if (!IsRemovable && !float.IsInfinity(data.Duration)) {
                        Destroy(Instance, data.Duration);
                    }
                }
            }
        }

        public override void Remove() {
            if (!IsRemovable || !IsApplied) return; // Is not removable/applied

            base.Remove();

            if (Instance != null && Instance) {
                Destroy(Instance);
            }
        }
    }
}
