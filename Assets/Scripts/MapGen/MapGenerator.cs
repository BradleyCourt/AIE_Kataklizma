using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MapGen {
    public class MapGenerator : MonoBehaviour {

        [Serializable]
        public enum TileType {
            Tile5m,
            Tile10m,
            Tile20m,
            Tile30m,
            Tile40m,
            TileRoad
        }

        [Serializable]
        public class SourceTilePreset {
            public TileType Type;
            public List<MapTile> Tiles;

        }

        public List<SourceTilePreset> SourceTilePresets;


    }
}

