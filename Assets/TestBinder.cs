using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kataklizma.Gameplay {

    public class TestBinder : MonoBehaviour {

        public Scriptables.ScriptedAbility NewAbility;
        public int AbilitySlot;

        public bool Ready;

        void Awake()
        {
#if DEBUG
#else
            Destroy(this);
#endif
        }

        // Update is called once per frame
        void Update() {

            if (Ready)
            {
                if ( NewAbility != null )
                    GetComponent<PlayerController>().SetAbilitySlot(AbilitySlot, NewAbility);

                NewAbility = null;
                AbilitySlot = 0;
                Ready = false;
            }
        }
    }
}