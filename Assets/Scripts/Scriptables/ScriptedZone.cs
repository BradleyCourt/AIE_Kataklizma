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

        public bool ScaleWithAnchor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trs">Translate, Rotate, Scale matrix</param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public abstract Collider[] OverlapZone(Matrix4x4 trs, int layerMask = ~8);
    }
}
