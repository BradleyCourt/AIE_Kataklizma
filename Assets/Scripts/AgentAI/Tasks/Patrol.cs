﻿// Patrol.cs
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
    private List<Gameplay.MapGen.PointOfInterest> points = null;
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

        player = GameObject.FindGameObjectWithTag("Player");

        //agent.SetDestination(player.transform.position);
    }

    void GotoNextPoint()
    {
        // if points is empty, give it points
        if (points == null )
            points = new List<Gameplay.MapGen.PointOfInterest>(FindObjectsOfType<Gameplay.MapGen.PointOfInterest>());

        // Do nothing if the array is empty
        if (points.Count == 0)
            return;


        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = Random.Range(0, points.Count);


        // Goto the selected destination
        agent.destination = points[destPoint].transform.position;
        agent.isStopped = false;
        // Pull random element from array
    }


    void Update()
    {
         if (!agent.isOnNavMesh) return;

        if (Vector3.Distance(agent.destination, transform.position) < 2) // No set destination
            GotoNextPoint();

        // Choose the next destination point when the agent gets
        // close to the current one.
        float dist = Vector3.Distance(transform.position, player.transform.position);

            if (!agent.pathPending)
            {
                // TODO - do we have line of sight? if we dont have line of sight, keep patrolling, if we do have line of sight, skip to the second step
          
                if (dist > 30)
                {
                    Target = null;
                    // TODO using the POI system, if target is not in range, select a random point and traverse to that point
                    // very far away, keep patrolling
                    Debug.DrawRay(transform.position, agent.destination - transform.position, Color.yellow);

                    if (agent.remainingDistance < 0.1f)
                        GotoNextPoint();
                }
                else if (dist > ChaseDist)
                {
                    // move closer to player
                    agent.SetDestination(player.transform.position);
                    //  Target = player.transform;
                    TargetPlayer(ChaseDist);
                }
                else if (dist < ChaseDist)
                {
                    //shoot mah chalie sheen gun
                    //GotoNextPoint();
                    Target = player.transform;


                    // stop and fire
                    //agent.Stop(); // we're within 5m so stop
                    // TODO COMMENT BACK IN WHEN INTEGRATING TANK SHELL FIRE agent.isStopped = true;

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