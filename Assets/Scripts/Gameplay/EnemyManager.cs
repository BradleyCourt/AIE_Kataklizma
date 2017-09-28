using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Gameplay.EntityAttributes playerAttributes;    // Reference to the player's heatlh.
    public GameObject enemy;                // The enemy prefab to be spawned.
    public float spawnTime = 3f;            // How long between each spawn.

    
    private List<Gameplay.MapGen.PointOfInterest> points = null;// List of all points of interests


    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn()
    {
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
        var go = Instantiate(enemy, poi.transform.position, poi.transform.rotation, transform);
        go.GetComponent<WarpOnce>().WarpNearestTo = poi.transform.position;

    }
}