using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "ConicZone", menuName = "Kataklizma/Zones/Conic Zone")]
    public class ConicZone : ScriptedZone {

        [Space]
        public float NearRadius;
        public float FarRadius;
        public float Length;


        protected bool OffsetsKnown = false;

        protected Vector3 OriginDelta;
        protected float SphereRadius;
        protected float ConeHalfangle;
        protected float ConeDotMinimum;

        protected void FindOffsets() {
            
            ConeHalfangle = Mathf.Atan(FarRadius / Length);
            ConeDotMinimum = Mathf.Sin(ConeHalfangle);
            
            OriginDelta = new Vector3();
            OriginDelta.z = -Length * NearRadius / (FarRadius - NearRadius);

            SphereRadius = new Vector2(Length + OriginDelta.z, FarRadius).magnitude;

            OffsetsKnown = true;
        }

        

        /// <summary>
        /// Discover all Colliders within zone
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="layerMask">Default: Ignore "PlayerAvater" layers</param>
        /// <returns></returns>
        public override Collider[] OverlapZone(Vector3 position, Quaternion rotation, int layerMask = ~8) {

            if (!OffsetsKnown) FindOffsets();

            var derivedOrigin = position + (rotation * OriginDelta);

            var range = Physics.OverlapSphere(derivedOrigin, SphereRadius, layerMask);

            var results = new List<Collider>();

            foreach (var collider in range) {
                if (collider == null) continue;
                if (!CanAffectTags.Contains(collider.gameObject.tag)) continue;

                // Check collider is within angle and min range
                var direction = (collider.transform.position - derivedOrigin).normalized;
                var dot = Vector3.Dot(direction, (rotation * Vector3.forward));

                if (dot < ConeDotMinimum || direction.magnitude < OriginDelta.z) continue;

                // Add to list
                results.Add(collider);
            }

            return results.ToArray();
        }
    }
}
