using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scriptables {

    [CreateAssetMenu(fileName ="MapTiles", menuName = "Kataklizma/Sets/Map Tile Collection")]
    public class MapTileCollection : ScriptableObject {

        [System.Serializable]
        public struct RoadPrefabs {

            public List<GameObject> End;
            public List<GameObject> Inter;
            public List<GameObject> Straight;
            public List<GameObject> Tee;
            public List<GameObject> Turn;

            public GameObject this[RoadType index] {
                get {
                    switch (index) {
                        case RoadType.End: return End[Random.Range(0, End.Count)];
                        case RoadType.Inter: return Inter[Random.Range(0, Inter.Count)];
                        case RoadType.Straight: return Straight[Random.Range(0, Straight.Count)];
                        case RoadType.Tee: return Tee[Random.Range(0, Tee.Count)];
                        case RoadType.Turn: return Turn[Random.Range(0, Turn.Count)];
                        default: throw new System.IndexOutOfRangeException();
                    }
                }
            }
        }

        [System.Serializable]
        public class StructurePrefabs {
            public MapTileType Size;
            public List<GameObject> Tiles;

            public GameObject GetRandomTile() {
                int index = Random.Range(0, Tiles.Count);
                return Tiles[index];
            }
        }

        public RoadPrefabs Roads;
        public List<StructurePrefabs> Structures;
    }
}