using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kataklizma.Gameplay.Singletons {
    public class StructuresDatabase : MonoBehaviour {

        protected static StructuresDatabase _Instance = null;
        public static StructuresDatabase Instance {
            get {
                return _Instance;
            }
            set {
                if (_Instance == null) _Instance = value;
            }
        }


        void Awake() {

        }
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}