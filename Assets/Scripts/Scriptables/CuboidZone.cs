﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "CuboidZone", menuName = "Volumes/Cuboid Zone")]
    public class CuboidZone : AbstractZone {

        public Bounds Bounds;

        public GameObject DebuggingPrefab;

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


#if DEBUG
            if (DebuggingPrefab != null) {
                // Render Zone
                // FIXME: REmove Zone Render
                var go = Instantiate(DebuggingPrefab, centre, origin.rotation);
                go.transform.localScale = size;

                Destroy(go, 1);
            }
#endif



            return Physics.OverlapBox(centre, size, origin.rotation, layerMask);
            
        }
    }
}
