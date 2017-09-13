// Patrol.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
public class Patrol : MonoBehaviour
{
    [HideInInspector]
    public float ChaseDist = 15;
    public Transform Target;
    public GameObject NavPointCollection;
    private List<Transform> points = new List<Transform>();
    private int destPoint = 0;
    private NavMeshAgent agent;
    private GameObject player;


    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        GotoNextPoint();
        player = GameObject.FindGameObjectWithTag("Player");

        //agent.SetDestination(player.transform.position);
    }

    void GotoNextPoint()
    {


        if (NavPointCollection == null) throw new System.ApplicationException(gameObject + " - Patrol: NavPointCollection cannot be empty");
        if (NavPointCollection.transform.childCount < 1) throw new System.ApplicationException(gameObject.name + " - Patrol: NavPointCollection must have children");

        for (var idx = 0; idx < NavPointCollection.transform.childCount; idx++)
        {
            points.Add(NavPointCollection.transform.GetChild(idx));
        }

        // Do nothing if the array is empty
        if (points.Count == 0)
            return;


        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = Random.Range(0, points.Count);


        // Goto the selected destination
        agent.destination = points[destPoint].position;
        agent.isStopped = false;
        // Pull random element from array:
        //var selected = points[Random.Range(0, points.Length)];
    }


    void Update()
    {
         if (!agent.isOnNavMesh) return;

        // Choose the next destination point when the agent gets
        // close to the current one.
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (!agent.pathPending)
        {
            // TODO - do we have line of sight? if we dont have line of sight, keep patrolling, if we do have line of sight, skip to the second step
            if (dist > 30)
            {
                Target = null;

                // very far away, keep patrolling
                if (agent.remainingDistance < 0.1f)
                    GotoNextPoint();
            }
            else if (dist > ChaseDist)
            {
                // move closer to player
                agent.SetDestination(player.transform.position);
                //  Target = player.transform;
                TargetPlayer(ChaseDist);
                //agent.Resume();
                agent.isStopped = false;
            }
            else
            {
                //shoot mah chalie sheen gun
                Target = player.transform;

                // stop and fire
                //agent.Stop(); // we're within 5m so stop
                agent.isStopped = true;

                // if within 3 metres, move away
                // else if player is stationary, shoot cannon


            }

        }
    }

    void SetDestination(Vector3 pt)
    {

    }
    void TargetPlayer(float ChaseDist)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, ChaseDist))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                Target = hit.collider.gameObject.transform;
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);

            }
        }
    }
}