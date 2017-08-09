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

    void Start()
    {
      
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
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


        // Goto the selected destination
        agent.destination = points[destPoint].position;
        agent.Resume();
        // Pull random element from array:
        //var selected = points[Random.Range(0, points.Length)];
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (!agent.pathPending)
        {
            // TODO - do we have line of sight? if we dont have line of sight, keep patrolling, if we do have line of sight, skip to the second step
            if (dist > 10)
            {
                // very far away, keep patrolling
                if (agent.remainingDistance < 0.1f)
                    GotoNextPoint();
            }
            else if (dist > 5)
            {
                // move closer to player
                agent.SetDestination(player.transform.position);
                agent.Resume();
            }
            else
            {
                //shoot mah chalie sheen gun

                // stop and fire
                agent.Stop(); // we're within 5m so stop
                Debug.Log("stopmoving");

                // if within 3 metres, move away
                // else if player is stationary, shoot cannon

             if (player.GetComponent<Rigidbody>().velocity.magnitude == 0)
                {
                    //shoot the cannon ball;
                }
            }

        }
    }

    void SetDestination(Vector3 pt)
    {

    }
}