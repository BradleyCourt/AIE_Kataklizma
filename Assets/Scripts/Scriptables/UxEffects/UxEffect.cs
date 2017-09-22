using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {
    public abstract class UxEffect : ScriptableObject {

        [Space]
        public bool Loop;

        public virtual void OnBegin() { }
        public virtual void OnUpdate() { }
        public virtual void OnEnd() { }

    }
}
