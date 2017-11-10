using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kataklizma.Gameplay;

public class EnemyManager : MonoBehaviour
{
    public EntityAttributes playerAttributes;    // Reference to the player's heatlh.
    public GameObject enemy;                // The enemy prefab to be spawned.
    public float spawnTime = 3f;            // How long between each spawn.
    public int SpawnCount;
    public float Elevation;

    protected Vector3 ElevationV3;

    private List<Gameplay.MapGen.PointOfInterest> points = null;// List of all points of interests


    void Start()
    {
        ElevationV3 = new Vector3(0, Elevation, 0);

        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn()
    {
        if (transform.childCount >= SpawnCount) return;

        // if points is empty, give it points
        if (points == null || points.Count == 0)
            points = new List<Gameplay.MapGen.PointOfInterest>(FindObjectsOfType<Gameplay.MapGen.PointOfInterest>());

        // Do nothing if the array is empty
        if (points.Count == 0)
            return;

        // If the player has no health left...
        if (playerAttributes[ValueType.Health] <= 0f)
        {
            // ... exit the function.

            return;
        }


        // Find a random index between zero and one less than the number of spawn points.


        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        //Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

        var poi = points[Random.Range(0, points.Count)];
        var go = Instantiate(enemy, poi.transform.position + ElevationV3, poi.transform.rotation, transform);
        go.GetComponent<WarpOnce>().WarpNearestTo = poi.transform.position + ElevationV3;

    }
}