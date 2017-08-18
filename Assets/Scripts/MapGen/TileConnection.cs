using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MapGen {
    public class TileConnection : MonoBehaviour {

        [Tooltip("Positive Z")]
        public bool Forward;

        [Tooltip("Negative Z")]
        public bool Back;

        [Tooltip("Negative X")]
        public bool Left;

        [Tooltip("Negative X")]
        public bool Right;
    }
}