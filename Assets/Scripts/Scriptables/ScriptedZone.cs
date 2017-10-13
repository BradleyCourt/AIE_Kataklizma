using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {
    public abstract class ScriptedZone : ScriptableObject {

        [UnityTag]
        [Tooltip("List of Tags that this ability will (attempt to) affect")]
        public List<string> CanAffectTags;

        /// <summary>
        /// Find all transforms in Zone
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public abstract Collider[] OverlapZone(Transform origin, int layerMask = ~8);
    }
}
