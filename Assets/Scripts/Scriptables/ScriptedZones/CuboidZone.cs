using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "CuboidZone", menuName = "Kataklizma/Zones/Cuboid Zone")]
    public class CuboidZone : ScriptedZone {

        [Space]
        public Bounds Bounds;

        //public GameObject DebuggingPrefab;

        /// <summary>
        /// Discover all Colliders within zone
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="layerMask">Default: Ignore "PlayerAvater" layers</param>
        /// <returns></returns>
        public override Collider[] OverlapZone(Vector3 position, Quaternion rotation, int layerMask = ~8) {

            var centre = position + (rotation * Bounds.center);
            var size = Bounds.extents;


#if DEBUG
            var prefab = Instantiate(Resources.Load<GameObject>("CuboidZonePrefab"), centre, rotation);
            if (prefab != null) {
                // Render Zone
                //var go = Instantiate(prefab, centre, origin.rotation);
                prefab.transform.localScale = 2 * size;

                Destroy(prefab, 0.8f);
            }
#endif



            return Physics.OverlapBox(centre, Bounds.extents, rotation, layerMask).Where(m => CanAffectTags.Contains( m.tag )).ToArray();
            
        }
    }
}
