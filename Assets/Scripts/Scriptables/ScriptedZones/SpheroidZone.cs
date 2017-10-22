using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "SpheroidZone", menuName = "Kataklizma/Zones/Spheroid Zone")]
    public class SpheroidZone : ScriptedZone {

        [Space]
        public Vector3 Centre;
        public Vector3 Radii;

        protected bool OffsetsKnown = false;

        protected float PrimeRadius;
        protected Vector3 Ratios;

        protected void FindOffsets() {

            PrimeRadius = Mathf.Max(Radii.x, Radii.y, Radii.z);
            Ratios = Radii / PrimeRadius;

            OffsetsKnown = true;
        }

        /// <summary>
        /// Discover all Colliders within zone
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="layerMask">Default: Ignore "PlayerAvater" layers</param>
        /// <returns></returns>
        public override Collider[] OverlapZone(Vector3 position, Quaternion rotation, int layerMask = ~8) {
            
            var range = Physics.OverlapSphere(position + Centre, PrimeRadius, layerMask);

            var results = new List<Collider>();

            foreach (var collider in range) {
                if (collider == null) continue;
                if (!CanAffectTags.Contains(collider.gameObject.tag)) continue;

                var worldOffset = collider.transform.position - position;
                //var localOffset = origin.transform.InverseTransformVector(worldOffset);
                var localOffset = Quaternion.Inverse(rotation) * worldOffset;

                // From https://stackoverflow.com/questions/17770555/how-to-check-if-a-point-is-inside-an-ellipsoid
                // (x/a)^2 + (y/b)^2 + (z/c)^2 <= 1

                var scaledOffset = Vector3.Scale(localOffset, Ratios);

                if (scaledOffset.magnitude > 1) continue;

                // Add to list
                results.Add(collider);
            }

            return results.ToArray();

        }
    }
}
