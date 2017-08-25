using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGen {
    [System.Serializable]
    public class MapTile {

        [System.Serializable]
        [System.Flags]
        public enum RoadConnections {
          //None = 0,
            Front = 1,
            Back = 2,
            Left = 4,
            Right = 8,
        }
        
        public GameObject TilePrefab;
        
    }
}