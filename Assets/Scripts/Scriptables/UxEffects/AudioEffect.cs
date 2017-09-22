using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "AudioEffect", menuName = "Kataklizma/Effects/Audio Effect")]
    public class AudioEffect : UxEffect {

        public string AudioName;

        public override void OnBegin() { }
        public override void OnUpdate() { }
        public override void OnEnd() { }

    }
}
