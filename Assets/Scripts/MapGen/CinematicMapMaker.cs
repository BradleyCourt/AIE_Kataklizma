using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MapGen
{

    public class CinematicMapMaker : MonoBehaviour
    {
        public Scriptables.MapTileCollection SourceTiles;

        public Vector2 Size = new Vector2(400,400);

        [Header("Roads")]
        public bool MakeRoads = true;
        public bool MakeRoadNavmesh = true;
        public int RoadLimit = 50;

        [Header("Skyway")]
        public bool MakeSkyway = true;
        public float SkywayHeight = 25;
                
        protected Transform Skyway;
        protected Transform Structures;
        protected Transform Roads;
        protected Transform Roadblocks;

        protected const float TileWidth = 5;
        protected const float TileHeight = 5;

        public Vector3 TileOriginOffset { get; protected set; }
        public int Columns { get; protected set; }
        public int Rows { get; protected set; }

        public GameObject[,] TileMap { get; protected set; }
        public bool[,] RoadMap { get; protected set; }

        const float AnimTime = 0.2f;
        #region " Utility "
        Vector3 IndexToWorld(int c, int r) {
            return TileOriginOffset + new Vector3(5 * c, 0, 5 * r);
        }

        int[] WorldToIndex(Vector3 pos) {

            pos -= TileOriginOffset;
            
            var c = (int)Mathf.Round(pos.x / 5);
            var r = (int)Mathf.Round(pos.z / 5);

            return new int[] { c, r };
        }

        private bool CanPlaceRoad(int ix, int iy, int crossChance) {
            return (TileMap[ix, iy] == null && (RoadMap[ix, iy] == false || UnityEngine.Random.Range(0, 100) < crossChance));
        }
        #endregion

        IEnumerator DiscoverStructures()
        {
            // we assume that all MapTilePresets belong to this grid because theres only one per scene
            MapTilePreset[] presets = FindObjectsOfType<MapTilePreset>();

            foreach (MapTilePreset preset in presets) {
                Vector3 pos = preset.transform.position;
                
                // snap origin to grid
                int[] indices = WorldToIndex(pos);

                // snap orientation to grid
                //preset.transform.position = IndexToWorld(indices[0], indices[1]);
                LeanTween.move(preset.gameObject, IndexToWorld(indices[0], indices[1]), AnimTime * 5).setEase(LeanTweenType.easeInOutBounce);
                yield return new WaitForSeconds(AnimTime * 5);
                

                

                // snap rotation to a 90 degrees value, assuming all buildings are children of the grid
                //preset.transform.localEulerAngles = new Vector3(0, Mathf.Floor((int)(preset.transform.localEulerAngles.y / 90.0f)) * 90.0f, 0);
                LeanTween.rotateLocal(preset.gameObject, new Vector3(0, Mathf.Round(preset.transform.localEulerAngles.y / 90.0f) * 90.0f, 0), AnimTime * 5).setEase(LeanTweenType.easeInOutBounce);
                yield return new WaitForSeconds(AnimTime * 5);

                Vector3 oppositeCorner = pos + (preset.transform.right + preset.transform.forward) * (int)(preset.Size) * 5;

                // Find opposite corner
                int[] indices2 = WorldToIndex(oppositeCorner);

                // find the minimum value of the bounds
                int startX = Mathf.Min(indices[0], indices2[0]);
                int startY = Mathf.Min(indices[1], indices2[1]);

                startX = Mathf.Max(startX, 0);
                startY = Mathf.Max(startY, 0);

                var endX = Mathf.Min(TileMap.GetLength(0), startX + (int)(preset.Size));
                var endY = Mathf.Min(TileMap.GetLength(1), startY + (int)(preset.Size));

                // fill in squares in gameobject array

                for (var c = startX; c < endX; c++)
                    for (var r = startY; r < endY; r++)
                        TileMap[c, r] = preset.gameObject;

                
            }
        }


        IEnumerator GenerateStructures() {

            // add buildings
            for (int c = 0; c < Columns; c++) {
                for (int r = 0; r < Rows; r++) {
                    if (TileMap[c, r] != null) continue;

                    // find all the pieces that fit and store in a local list
                    var onesThatFit = new List<Scriptables.MapTileCollection.StructurePrefabs>();
                    var slots = new Queue<int>();
                    var totalSlots = 0;


                    foreach (var pre in SourceTiles.Structures) {
                        // does it fit?
                        bool fits = true;
                        int w = (int)(pre.Size);

                        // Check Map Bounds
                        if (c + w - 1 >= Columns)
                            fits = false;
                        if (r + w - 1 >= Rows)
                            fits = false;

                        // check all squares that this one will fill, to make sure they're all empty
                        for (int i = 0; i < w && fits; i++) {
                            for (int j = 0; j < w && fits; j++) {
                                if (TileMap[c + i, r + j] != null)
                                    fits = false;
                            }
                        }

                        if (fits) {
                            onesThatFit.Add(pre);
                            slots.Enqueue(pre.Tiles.Count);
                            totalSlots += pre.Tiles.Count;
                        }
                    }


                    int slot = UnityEngine.Random.Range(0, totalSlots);

                    var index = 0;
                    while (slots.Count > 0 && slot >= slots.Peek()) { // Roulette Selection
                        slot -= slots.Dequeue();
                        index++;
                    }

                    var preset = onesThatFit[index];

                    GameObject prefab = preset.Tiles[slot];//.GetRandomTile();

                    // instansiates a prefab
                    GameObject go = Instantiate(prefab);
                    go.transform.parent = Structures;

                    go.transform.localPosition = TileOriginOffset + new Vector3(TileWidth * c, 0, TileHeight * r);
                    go.transform.localRotation = Quaternion.identity;

                    // TODO: Add random orientation

                    switch (UnityEngine.Random.Range(0,4)) {
                        case 0: // Do Nothing
                            break;
                        case 1: // 90-degree rotation
                            go.transform.position += new Vector3(0, 0, 5 * (int)preset.Size);
                            go.transform.Rotate(0, 90, 0);
                            break;
                        case 2: // 180-degree rotation
                            go.transform.position += new Vector3(5 * (int)preset.Size, 0, 5 * (int)preset.Size);
                            go.transform.Rotate(0, 180, 0);
                            break;
                        case 3: // 270-degree rotation
                            go.transform.position += new Vector3(5 * (int)preset.Size, 0, 0);
                            go.transform.Rotate(0, 270, 0);
                            break;
                    }

                    go.name += " [" + c.ToString("N3") + "," + r.ToString("N3") + "]";

                    //write to mapObjects array
                    for (int i = 0; i < (int)(preset.Size); i++) {
                        for (int j = 0; j < (int)(preset.Size); j++) {
                            TileMap[c + i, r + j] = go; // fills array
                        }
                    }

                    yield return new WaitForSeconds(AnimTime/4f);
                }

            }
        }

        IEnumerator GenerateRoads() {

            yield return GenerateRoadMap();
            yield return GenerateRoadTiles();
        }

        IEnumerator GenerateRoadMap() {
            var roadblocks = Instantiate(Resources.Load<GameObject>("EmptyPrefab"), transform);
            roadblocks.name = "Roadblocks";
            Roadblocks = roadblocks.transform;

            var cuboidZone = Resources.Load<GameObject>("CuboidZonePrefab");

            // prepick some starting values
            List<int> indicesX = new List<int>(RoadLimit);
            List<int> indicesY = new List<int>(RoadLimit);

            // fill these in -TODO

            int priorX = 0;
            int priorY = 0;

            for (int i = 0; i < RoadLimit; i++) {
                indicesX.Add(priorX += UnityEngine.Random.Range(3, 4)); // Generate offset from prior value, store in indeces and update prior value
                indicesY.Add(priorY += UnityEngine.Random.Range(3, 4));
            }

            //add roads

            int verticalCount = 0;
            const int verticalWeight = 5;

            for (int i = 0; i < RoadLimit; i++) {
                if (indicesX.Count == 0 || indicesY.Count == 0) break; // If run out of parallels, stop generating

                bool isVertical = (UnityEngine.Random.Range(0, 100) < 50 - verticalCount);
                int indexX, indexY;

                if (isVertical) {
                    indexX = indicesX[UnityEngine.Random.Range(0, indicesX.Count)];
                    indexY = UnityEngine.Random.Range(0, Rows);

                    indicesX.Remove(indexX);
                    verticalCount += verticalWeight; // increase chance of horizontal
                }
                else {
                    // get a random point (or use pre-picked values)
                    indexX = UnityEngine.Random.Range(0, Columns);
                    indexY = indicesY[UnityEngine.Random.Range(0, indicesY.Count)];

                    indicesY.Remove(indexY);
                    verticalCount -= verticalWeight; // decrease chance of horizontal
                }

                if (indexX >= Columns || indexY >= Rows) {
                    i--;
                    continue; // Conform to bounds
                }


                // move up and down* filling in road markers till we hit a road
                int ix = indexX;
                int iy = indexY;
                if (isVertical) {
                    // go up
                    while (iy < Rows && CanPlaceRoad(ix, iy, 30)) {
                        RoadMap[ix, iy] = true;
                        yield return CreateRoadblock(ix, iy, roadblocks, cuboidZone);
                        iy++;
                    }
                    // go back to start and go down
                    iy = indexY - 1;
                    while (iy >= 0 && CanPlaceRoad(ix, iy, 30)) {
                        RoadMap[ix, iy] = true;
                        yield return CreateRoadblock(ix, iy, roadblocks, cuboidZone);
                        iy--;
                    }
                }
                else {
                    // go up
                    while (ix < Columns && CanPlaceRoad(ix, iy, 30)) {
                        RoadMap[ix, iy] = true;
                        yield return CreateRoadblock(ix, iy, roadblocks, cuboidZone);
                        ix++;
                    }
                    // go back to start and go down
                    ix = indexX - 1;
                    while (ix >= 0 && CanPlaceRoad(ix, iy, 30)) {

                        RoadMap[ix, iy] = true;
                        yield return CreateRoadblock(ix, iy, roadblocks, cuboidZone);
                        ix--;
                    }
                }
            }
        }

        IEnumerator CreateRoadblock(int x, int y, GameObject collection, GameObject prefab) {
            GameObject go = Instantiate(prefab, collection.transform);
            go.transform.localPosition = IndexToWorld(x, y) + new Vector3(2.5f, 0, 2.5f);
            go.transform.localScale = new Vector3(5, 1, 5);

            go.name += "Roadblock [" + x.ToString("N") + "," + y.ToString("N") + "]";
            yield return new WaitForSeconds(AnimTime / 10f);
        }

        IEnumerator GenerateRoadTiles() {
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
            for (int c = 0; c < Columns; c++) {
                string line = "";
                for (int r = 0; r < Rows; r++) {
                    line += RoadMap[c, r] ? "o" : ".";

                    // if returns "o" (that means a road is there, do this code 
                    // {
                    if (RoadMap[c, r] && TileMap[c, r] == null) {
                        // Check adjacent tiles for other roads
                        var LeftCheck = c > 0 && RoadMap[c - 1, r];
                        var RightCheck = c < RoadMap.GetLength(0) - 1 && RoadMap[c + 1, r];
                        var BottomCheck = r > 0 && RoadMap[c, r - 1];
                        var TopCheck = r < RoadMap.GetLength(1) - 1 && RoadMap[c, r + 1];

                        var connections = 0;
                        if (LeftCheck) connections |= 1;
                        if (RightCheck) connections |= 2;
                        if (BottomCheck) connections |= 4;
                        if (TopCheck) connections |= 8;

                        connections &= 0xF; // Mask to first 4 bits

                        if (connections != 0) {
                            // Instantiate:               
                            GameObject prefab = SourceTiles.Roads[tiles[connections]];

                            GameObject go = Instantiate(prefab);
                            go.transform.parent = Roads;
                            go.transform.localPosition = IndexToWorld(c, r) + new Vector3(2.5f, 0, 2.5f); // TODO cheeky 5m hack, probably OK
                            go.transform.localEulerAngles = new Vector3(0, (int)angles[connections], 0);

                            go.name += " [" + c.ToString("N3") + "," + r.ToString("N3") + "]";

                            TileMap[c, r] = go; // fills array
                        }

                        yield return new WaitForSeconds(AnimTime / 20.0f);
                    }

                }
            }
            if (Roadblocks != null) Destroy(Roadblocks.gameObject);
        }

        IEnumerator DoWork() {
            yield return new WaitForSeconds(0.2f);

            Columns = (int)(Size.x / 5);
            Rows = (int)(Size.y / 5);

            TileMap = new GameObject[Columns, Rows];
            TileOriginOffset = new Vector3(5 * Columns * -0.5f, 0, 5 * Rows * -0.5f);

            // Start Coroutines
            yield return DiscoverStructures();

            if (MakeRoads) {
                RoadMap = new bool[Columns, Rows];

                // Make Parent
                var go = Instantiate(Resources.Load<GameObject>("EmptyPrefab"), transform);
                go.name = "Roads";
                Roads = go.transform;

                // Populate Roads
                yield return GenerateRoads();
            }
            // Make Structures
            {
                // Make Parent
                var go = Instantiate(Resources.Load<GameObject>("EmptyPrefab"), transform);
                go.name = "Structures";
                Structures = go.transform;

                // Poluate Structures
                yield return GenerateStructures();
            }


            if (MakeSkyway) {
                // Make Skyway (Has no children, so leave at top-level)
                var go = Instantiate(Resources.Load<GameObject>("SkywayPrefab"), transform);
                go.name = "Skyway";
                Skyway = go.transform;

                // Move and size Skyway
                Skyway.position += new Vector3(0, SkywayHeight, 0);
                Skyway.localScale = new Vector3(5 * Columns, 10, 5 * Rows) / 10;
            }


            if (MakeRoadNavmesh && Roads != null) {
                NavMeshGen.BuildNavMesh(Roads, new Bounds(transform.position, new Vector3(5 * Columns, 1, 5 * Rows)));
            }

            if (Skyway != null) { // Always generate NavMesh if Skyway exists
                NavMeshGen.BuildNavMesh(Skyway, new Bounds(Skyway.transform.position, new Vector3(5 * Columns, 1, 5 * Rows)));
            }
        }

        public void Start()
        {
            StartCoroutine(DoWork());
        }
    }
}

