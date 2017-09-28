using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scriptables {

    [CreateAssetMenu(fileName = "PrefabEffect", menuName = "Kataklizma/Effects/Particle Effect")]
    public class PrefabEffect : ScriptedEffect {

        public CharacterBindPoint Location;
        public GameObject Prefab;
        public bool RequiresLocalScale;

    }
}
