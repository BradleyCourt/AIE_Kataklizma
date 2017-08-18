using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables {
    [CreateAssetMenu(fileName = "Attack", menuName = "Abilities/AoE Attack", order = 1)]
    public class AttackArea : Attack {

        public enum AreaType {
            Sphere,
            Cuboid,
        }

        public AreaType Type;
        public Vector3 Centre;
        public Vector3 CuboidExtents;
        public float SphereRadius;

    }
}