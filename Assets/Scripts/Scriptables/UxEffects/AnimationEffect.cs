using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {
    [CreateAssetMenu(fileName = "AnimationEffect", menuName = "Kataklizma/Effects/Animation Effect")]
    public class AnimationEffect : UxEffect {

        public string AnimationName;

        public override void OnBegin() { }
        public override void OnUpdate() { }
        public override void OnEnd() { }

    }
}
