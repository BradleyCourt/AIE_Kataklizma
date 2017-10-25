
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGen : MonoBehaviour {

    public Vector3 Size;
    public static List<NavMeshDataInstance> instances =  new List<NavMeshDataInstance>();

    void Start()
    {
       
        BuildNavMesh(transform, new Bounds(transform.position, Size));
    }

    public static void BuildNavMesh(Transform root, Bounds bounds)
    {
        // Use the standard settings from the editor (I think)
        NavMeshBuildSettings settings = NavMesh.GetSettingsByID(0);

        // gather all the physics colliders which are children of this transform (or you can do this by volume)
        List<NavMeshBuildSource> results = new List<NavMeshBuildSource>();

        NavMeshBuilder.CollectSources(root, 255, NavMeshCollectGeometry.RenderMeshes, 0, new List<NavMeshBuildMarkup>(), results);
        
        // Build the actual navmesh
        NavMeshData data = NavMeshBuilder.BuildNavMeshData(settings, results, bounds, Vector3.zero, Quaternion.identity);
        instances.Add(NavMesh.AddNavMeshData(data));
        
        //success = NavMeshBuilder.UpdateNavMeshData(data, settings, results, bounds);
    }
    public static void ClearNavMesh()
    {
        foreach(var instance in instances)
            NavMesh.RemoveNavMeshData(instance);

        instances.Clear();
    }
}
