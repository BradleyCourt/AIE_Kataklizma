using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scriptables {

    [CreateAssetMenu(fileName ="Mutator Set", menuName = "Kataklizma/Sets/Mutator Collection")]
    public class MutatorCollection : ScriptableObject {

        List<ScriptedAbility> Abilities;

    }
}