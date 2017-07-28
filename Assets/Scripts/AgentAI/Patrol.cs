// Patrol.cs
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private GameObject player;
    private bool isInRange;
    void Start()
    {
        bool isInRange = false;
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        //GotoNextPoint();
        player = GameObject.FindGameObjectWithTag("Player");

        agent.SetDestination(player.transform.position);
    }


    void GotoNextPoint()
    {
        // Do nothing if the array is empty
        if (points.Length == 0)
            return;


        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = Random.Range(0, points.Length);
        Debug.Log(destPoint);


        // Goto the selected destination
        agent.destination = points[destPoint].position;

        // Pull random element from array:
        //var selected = points[Random.Range(0, points.Length)];
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance > 5)// && isInRange != true)
            //GotoNextPoint();
            agent.SetDestination(player.transform.position);
        else if (!agent.pathPending && agent.remainingDistance < 5)// && isInRange != true)
        {
           //isInRange = true;
            agent.SetDestination(agent.transform.position);
            Debug.Log("stopmoving");
        }


    }
}