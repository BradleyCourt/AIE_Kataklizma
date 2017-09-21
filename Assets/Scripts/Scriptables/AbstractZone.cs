using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {
    public abstract class AbstractZone : ScriptableObject {

        /// <summary>
        /// Find all transforms in Zone
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public abstract Collider[] OverlapZone(Transform origin, int layerMask = ~8);
    }
}
