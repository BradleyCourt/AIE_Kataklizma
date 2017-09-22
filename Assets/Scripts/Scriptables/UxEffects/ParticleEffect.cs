using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "ParticleEffect", menuName = "Kataklizma/Effects/Particle Effect")]
    public class ParticleEffect : UxEffect {

        public CharacterBindPoint Location;
        public GameObject Prefab;


        public override void OnBegin() { }
        public override void OnUpdate() { }
        public override void OnEnd() { }

    }
}
