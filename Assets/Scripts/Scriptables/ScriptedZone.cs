using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {
    public abstract class ScriptedZone : ScriptableObject {

        [UnityTag]
        [Tooltip("List of Tags that this ability will (attempt to) affect")]
        public List<string> CanAffectTags = new List<string> { "Building", "Enemy" };


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public abstract Collider[] OverlapZone(Vector3 position, Quaternion rotation, int layerMask = ~8);
    }
}
