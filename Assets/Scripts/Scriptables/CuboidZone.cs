using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "CuboidZone", menuName = "Volumes/Cuboid Zone")]
    public class CuboidZone : AbstractZone {

        public Bounds Bounds;

        /// <summary>
        /// Discover all Colliders within zone
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="layerMask">Default: Ignore "PlayerAvater" layers</param>
        /// <returns></returns>
        public override Collider[] OverlapZone(Transform origin, int layerMask = ~8) {
            //throw new NotImplementedException();

            var centre = origin.TransformPoint(Bounds.center);
            var size = Vector3.Scale(Bounds.extents, origin.localScale);

            return Physics.OverlapBox(centre, size, origin.rotation, layerMask);
            
        }
    }
}
