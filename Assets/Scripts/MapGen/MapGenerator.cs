using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MapGen
{

    public class MapGenerator : MonoBehaviour
    {

        [System.Serializable]
        public struct RoadPrefabs
        {
            public List<GameObject> End;
            public List<GameObject> Inter;
            public List<GameObject> Straight;
            public List<GameObject> Tee;
            public List<GameObject> Turn;

            public GameObject this[int index] {
                get
                {
                    switch (index)
                    {
                        case 0: return End[UnityEngine.Random.Range(0, End.Count)];
                        case 1: return Turn[UnityEngine.Random.Range(0, Turn.Count)];
                        case 2: return Straight[UnityEngine.Random.Range(0, Straight.Count)];
                        case 3: return Tee[UnityEngine.Random.Range(0, Tee.Count)];
                        case 4: return Inter[UnityEngine.Random.Range(0, Inter.Count)];
                        default: throw new System.IndexOutOfRangeException();
                    }
                }
            }
        }

        public enum RoadType
        {
            _Invalid = -1,
            End = 0,
            Turn = 1,
            Straight = 2,
            Tee = 3,
            Inter = 4
        }

        public enum RoadRotation
        {
            _Invalid = -1,
            Turn000 = 0,
            Turn090 = 90,
            Turn180 = 180,
            Turn270 = 270,
        }



        [Serializable]
        public class SourceTilePreset
        {
            public MapTileType Type;
            public List<GameObject> Tiles;

            public GameObject GetRandomTile()
            {
                int index = UnityEngine.Random.Range(0, Tiles.Count);
                return Tiles[index];
            }

            public int getWidth()
            {
                return getTileWidth(Type);
            }
        }

        public static int getTileWidth(MapTileType Type)
        {
            switch (Type)
            {
                case MapTileType.Tile5m: return 1;
                case MapTileType.Tile10m: return 2;
                case MapTileType.Tile20m: return 4;
                case MapTileType.Tile30m: return 6;
                case MapTileType.Tile40m: return 8;
                case MapTileType.Tile80m: return 16;
                    // TODO  all other sizes
            }
            return -1; // this should never happen!
        }

        public Vector2 bounds;

        private Vector3 TileOriginOffset;

        private Transform Structures;
        private Transform Roads;
        /// <summary>
        /// 
        /// </summary>
        public List<SourceTilePreset> SourceTilePresets;
   

        public RoadPrefabs RoadTilePresets;

        /// <summary>
        ///
        /// </summary>
        public void GenerateGrid()
        {
            int columns = (int)bounds.x;
            int rows = (int)bounds.y;



            // make an array of gameobjects
            GameObject[,] mapObjects = new GameObject[columns, rows];

            AddPremadeBuildings(mapObjects);
            AddRoads(mapObjects);
            AddBuildings(mapObjects);
         
        }

        void AddPremadeBuildings(GameObject[,] mapObjects)
        {
            // we assume that all MapTilePresets belong to this grid because theres only one per scene
            MapTilePreset[] presets = FindObjectsOfType<MapTilePreset>();

            foreach (MapTilePreset preset in presets)
            {
                Vector3 pos = preset.transform.position;
                int width = getTileWidth(preset.Size);
                

                // snap rotation to a 90 degrees value, assuming all buildings are children of the grid
                preset.transform.localEulerAngles = new Vector3(0, Mathf.Floor((int)(preset.transform.localEulerAngles.y / 90.0f)) * 90.0f, 0);

                Vector3 oppositeCorner = pos + (preset.transform.right + preset.transform.forward) * width * 5;

                // snap the position to the grid
                int[] indices = WorldToIndex(pos);

                // snap the object to the grid
                preset.transform.position = IndexToWorld(indices[0], indices[1]);



                int[] indices2 = WorldToIndex(oppositeCorner);

                // find the minimum value of the bounds
                int startX = Mathf.Min(indices[0], indices2[0]);
                int startY = Mathf.Min(indices[1], indices2[1]);

                startX = Mathf.Max(startX, 0);
                startY = Mathf.Max(startY, 0);

                var endX = Mathf.Min(mapObjects.GetLength(0), startX + width);
                var endY = Mathf.Min(mapObjects.GetLength(1), startY + width);

                // fill in squares in gameobject array

                for (var c = startX; c < endX ; c++)
                    for (var r = startY; r < endY; r++)
                        mapObjects[c, r] = preset.gameObject;




            }
        }

        void AddRoads(GameObject[,] mapObjects)
        {
            int columns = (int)bounds.x;
            int rows = (int)bounds.y;

            bool[,] isRoad = new bool[columns, rows];

            int numRoads = 70;

            // prepick some starting values
            List<int> indicesX = new List<int>(numRoads);
            List<int> indicesY = new List<int>(numRoads);

            // fill these in -TODO

            int priorX = 0;
            int priorY = 0;

            for (int i = 0; i < numRoads; i++)
            {
                indicesX.Add(priorX += UnityEngine.Random.Range(3, 4)); // Generaete offset from prior value, store in indeces and update prior value
                indicesY.Add(priorY += UnityEngine.Random.Range(3, 4));
            }

            //add roads

            int verticalCount = 0;
            int verticalWeight = 5;

            for (int i = 0; i < numRoads; i++)
            {
                if (indicesX.Count == 0 || indicesY.Count == 0) break; // If run out of parallels, stop generating

                bool isVertical = (UnityEngine.Random.Range(0, 100) < 50 - verticalCount);
                int indexX, indexY;

                if (isVertical)
                {
                    indexX = indicesX[UnityEngine.Random.Range(0, indicesX.Count)];
                    indexY = UnityEngine.Random.Range(0, rows);

                    indicesX.Remove(indexX);
                    verticalCount += verticalWeight; // increase chance of horizontal
                }
                else
                {
                    // get a random point (or use pre-picked values)
                    indexX = UnityEngine.Random.Range(0, columns);
                    indexY = indicesY[UnityEngine.Random.Range(0, indicesY.Count)];

                    indicesY.Remove(indexY);
                    verticalWeight -= verticalWeight; // decrease chance of horizontal
                }

                if (indexX >= bounds.x || indexY >= bounds.y)
                {
                    i--;
                    continue; // Conform to bounds
                }


                // move up and down* filling in road markers till we hit a road
                int ix = indexX;
                int iy = indexY;
                if (isVertical)
                {
                    // go up
                    while (iy < rows && CanPlaceRoad(mapObjects, isRoad, ix, iy, 30))
                    {
                        isRoad[ix, iy] = true;
                        iy++;
                    }
                    // go back to start and go down
                    iy = indexY - 1;
                    while (iy >= 0 && CanPlaceRoad(mapObjects, isRoad, ix, iy, 30))
                    {
                        isRoad[ix, iy] = true;
                        iy--;
                    }
                }
                else
                {
                    // go up
                    while (ix < columns && CanPlaceRoad(mapObjects, isRoad, ix, iy, 30))
                    {
                        isRoad[ix, iy] = true;
                        ix++;
                    }
                    // go back to start and go down
                    ix = indexX - 1;
                    while (ix >= 0 && CanPlaceRoad(mapObjects, isRoad, ix, iy, 30))
                    {
                        isRoad[ix, iy] = true;
                        ix--;
                    }
                }

            }


            // make sure these indices match in the inspector or it wont work!!!
            // tile indexs should be 0 = end, 1 = turn, 2 = straight, 3 = Tjunction, 4 = intersection. -1 means nothing at all

            // use the correct tiles to make the map gen look natural
            // first index = null
            RoadType[] tiles = {
                            RoadType._Invalid,
                            RoadType.End,  // Tile 1
                            RoadType.End,  // Tile 2
                            RoadType.Straight,  // Tile 3
                            RoadType.End, // Tile 4
                            RoadType.Turn, // Tile 5
                            RoadType.Turn,  // Tile 6
                            RoadType.Tee,  // Tile 7
                            RoadType.End,  // Tile 8
                            RoadType.Turn, // Tile 9
                            RoadType.Turn,  // Tile 10
                            RoadType.Tee,  // Tile 11
                            RoadType.Straight,  // Tile 12
                            RoadType.Tee,  // Tile 13
                            RoadType.Tee,  // Tile 14
                            RoadType.Inter, // Tile 15
                        };

            // have angles match the rotation of the pieces listed above
            // first index = null

            RoadRotation[] angles = {
                            RoadRotation._Invalid,
                            RoadRotation.Turn090,  // Tile 1
                            RoadRotation.Turn270,  // Tile 2
                            RoadRotation.Turn270,  // Tile 3
                            RoadRotation.Turn000,  // Tile 4
                            RoadRotation.Turn000,  // Tile 5
                            RoadRotation.Turn270,  // Tile 6
                            RoadRotation.Turn180,  // Tile 7
                            RoadRotation.Turn180,  // Tile 8
                            RoadRotation.Turn090,  // Tile 9
                            RoadRotation.Turn180,  // Tile 10
                            RoadRotation.Turn000,  // Tile 11
                            RoadRotation.Turn000,  // Tile 12
                            RoadRotation.Turn270,  // Tile 13
                            RoadRotation.Turn090,  // Tile 14
                            RoadRotation.Turn270,  // Tile 15
                        };

            // quick printout (debug)
            for (int c = 0; c < columns; c++)
            {
                string line = "";
                for (int r = 0; r < rows; r++)
                {
                    line += isRoad[c, r] ? "o" : ".";

                    // if returns "o" (that means a road is there, do this code 
                    // {
                    if (isRoad[c, r] && mapObjects[c,r] == null)
                    {
                        // Check adjacent tiles for other roads
                        var LeftCheck = c > 0 && isRoad[c - 1, r];
                        var RightCheck = c < isRoad.GetLength(0) - 1 && isRoad[c + 1, r]; // should work tm (get length)
                        var BottomCheck = r > 0 && isRoad[c, r - 1];
                        var TopCheck = r < isRoad.GetLength(1) - 1 && isRoad[c, r + 1];

                        var connections = 0;
                        if (LeftCheck) connections |= 1;
                        if (RightCheck) connections |= 2;
                        if (BottomCheck) connections |= 4;
                        if (TopCheck) connections |= 8;

                        connections &= 15; // Mask to first 4 bits
                        
                        if (connections != 0)
                        {
                            // Instantiate:               
                            GameObject prefab = RoadTilePresets[(int)tiles[connections]];

                            GameObject go = Instantiate(prefab);
                            go.transform.parent = Roads;
                            go.transform.localPosition = IndexToWorld(c,r) + new Vector3(2.5f, 0, 2.5f); // TODO cheeky 5m hack, probably OK
                            go.transform.localEulerAngles = new Vector3(0, (int)angles[connections], 0);
                            go.name = "MapTile_" + c + "_" + r;

                            mapObjects[c, r] = go; // fills array
                        }
                    }
                    // }
                }
                //Debug.Log(line);
            }
            
        }

        Vector3 IndexToWorld(int c, int r)
        {
            return TileOriginOffset + new Vector3(5 * c, 0, 5 * r);
        }

        int[] WorldToIndex(Vector3 pos)
        {
            int[] indices = new int[2];

            pos -= TileOriginOffset;

            // 0.5f's here mean we round off instead of rounding down
            indices[0] = (int)(Mathf.Floor(pos.x) / 5);
            indices[1] = (int)(Mathf.Floor(pos.z) / 5);
            
            return indices;
        }


        private bool CanPlaceRoad(GameObject[,] mapObjects, bool[,] isRoad, int ix, int iy, int crossChance)
        {
            return (mapObjects == null || mapObjects[ix,iy] == null) && (isRoad[ix, iy] == false || UnityEngine.Random.Range(0, 100) < crossChance);
        }

        void AddBuildings(GameObject[,] mapObjects)
        {
            int columns = (int)bounds.x;
            int rows = (int)bounds.y;

            // add buildings
            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    if (mapObjects[c, r] == null) // if squares empty, put it in
                    {
                        // find all the pieces that fit and store in a local list
                        List<SourceTilePreset> onesThatFit = new List<SourceTilePreset>();

                        foreach (SourceTilePreset pre in SourceTilePresets)
                        {
                            // does it fit?
                            bool fits = true;
                            int w = pre.getWidth();
                            if (c + w - 1 >= columns)
                                fits = false;
                            if (r + w - 1 >= rows)
                                fits = false;

                            // check all squares that this one will fill, to make sure they're all empty
                            if (fits)
                            {
                                for (int i = 0; i < w; i++)
                                {
                                    for (int j = 0; j < w; j++)
                                    {
                                        if (mapObjects[c + i, r + j] != null)
                                            fits = false;
                                    }
                                }
                            }
                            if (fits)
                            {
                                onesThatFit.Add(pre);
                            }
                        }

                        int index = UnityEngine.Random.Range(0, onesThatFit.Count);
                        SourceTilePreset preset = onesThatFit[index];
                        GameObject prefab = preset.GetRandomTile();

                        // instansiates a prefab
                        GameObject go = Instantiate(prefab);
                        go.transform.parent = Structures;
                        go.transform.localPosition = TileOriginOffset + new Vector3(5 * c, 0, 5 * r); // TODO cheeky 5m hack, probably OK
                        go.transform.localRotation = Quaternion.identity;
                        go.name = "MapTile_" + c + "_" + r;

                        //write to mapObjects array
                        int width = preset.getWidth();
                        for (int i = 0; i < width; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                mapObjects[c + i, r + j] = go; // fills array
                            }
                        }
                   } 
                }
            }
        }
        void Start()
        {

            // Generate Roads
            // Fill buildings
            // Genenerate Road and Skyway Navmeshes


            Structures = transform.Find("Structures") ?? transform; // Find "Structures" child, else use "this" transform
            Roads = transform.Find("Roads") ?? transform;
            var Skyway = transform.Find("Skyway");

            TileOriginOffset = new Vector3(5 * bounds.x * -0.5f, 0, 5 * bounds.y * -0.5f);
            GenerateGrid();

            NavMeshGen.BuildNavMesh(Roads, new Bounds(transform.position, new Vector3(5 * bounds.x, 1, 5 * bounds.y)));

            if (Skyway != null )
                NavMeshGen.BuildNavMesh(Skyway, new Bounds(Skyway.transform.position, new Vector3(5 * bounds.x, 1, 5 * bounds.y)));
        }
    }
}

