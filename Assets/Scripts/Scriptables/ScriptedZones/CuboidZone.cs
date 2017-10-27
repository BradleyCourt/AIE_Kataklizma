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
        public override Collider[] OverlapZone(Matrix4x4 trs, int layerMask = ~8) {
            
            var position = trs.MultiplyPoint(Bounds.center);
            var rotation = trs.GetRotation();
            var scale = trs.GetScale();

            var extents = Vector3.Scale(Bounds.extents, scale);
            

#if DEBUG
            var prefab = Instantiate(Resources.Load<GameObject>("CuboidZonePrefab"), position, rotation);
            if (prefab != null) {
                // Render Zone
                prefab.transform.localScale = extents;

                Destroy(prefab, 0.8f);
            }
#endif
            

            return Physics.OverlapBox(position, extents/2, rotation, layerMask).Where(m => CanAffectTags.Contains( m.tag )).ToArray();
            
        }


    }
}
