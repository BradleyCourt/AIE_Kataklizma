using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {
    public abstract class ScriptedEffect : ScriptableObject {
        
        public class Params {

            public float Duration = float.PositiveInfinity;
            public Collider[] Hits = null;

            Dictionary<string, object> OtherValues = new Dictionary<string, object>();
        }
        
        protected Transform Owner;

        protected bool IsApplied;

        public bool IsRemovable = true;

        public virtual ScriptedEffect CloneAndBind(Transform owner)
        {
            var result = Instantiate(this);
            result.Bind(owner);
            return result;
        }

        public virtual bool Bind(Transform owner) {
            if (owner == null) { Unbind(); return false; }

            Owner = owner;
            
            return true;
        }

        public virtual void Unbind() {
            if (Owner == null) return;

            Owner = null;
        }

        public virtual void Apply(Params data) { IsApplied = true; }
        public virtual void Remove() { IsApplied = false; }
    }
}
