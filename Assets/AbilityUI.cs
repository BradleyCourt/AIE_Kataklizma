using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kataklizma.Gameplay {
    public class AbilityUI : MonoBehaviour {


        protected PlayerController Controller;
        protected List<GameObject> Items;


        // Use this for initialization
        void Start() {

            Controller = FindObjectOfType<PlayerController>();
    
        }

        // Update is called once per frame
        void Update() {

        }
    }
}