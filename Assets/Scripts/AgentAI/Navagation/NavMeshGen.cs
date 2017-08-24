
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGen : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        BuildNavMesh(transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    void BuildNavMesh(Transform root)
    {
        // Use the standard settings from the editor (I think)
        NavMeshBuildSettings settings = NavMesh.GetSettingsByID(0);

        // gather all the physics colliders which are children of this transform (or you can do this by volume)
        List<NavMeshBuildSource> results = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(root, 255, NavMeshCollectGeometry.RenderMeshes, 0, new List<NavMeshBuildMarkup>(), results);

        // make a 100m box around the origin
        Bounds bounds = new Bounds(Vector3.zero, 100 * Vector3.one);

        // Build the actual navmesh
        NavMeshData data = NavMeshBuilder.BuildNavMeshData(settings, results, bounds, Vector3.zero, Quaternion.identity);
        NavMesh.AddNavMeshData(data);
        //success = NavMeshBuilder.UpdateNavMeshData(data, settings, results, bounds);
    }
}
