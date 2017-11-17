// Patrol.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
public class Patrol : MonoBehaviour
{
    
    public float ChaseDist = 15;
    public float MinApproach = 5;
    public Transform Target;
    public GameObject NavPointCollection;
    private List<Gameplay.MapGen.PointOfInterest> points = null;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private GameObject player;

    public float NavTimeout = 2;
    protected float NavRescanAfter;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        player = GameObject.FindGameObjectWithTag("Player");

        //agent.SetDestination(player.transform.position);

        NavRescanAfter = Time.time;
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
        var projectedPoint = points[destPoint].transform.position;
        projectedPoint.y = transform.position.y;

        agent.destination = projectedPoint;
        agent.isStopped = false;

        NavRescanAfter = Time.time + NavTimeout;

        // Pull random element from array
    }


    void Update()
    {
        if (!agent.isOnNavMesh) return;

        if (agent.destination == transform.position) // No set destination
            GotoNextPoint();
        
        // Choose the next destination point when the agent gets
        // close to the current one.
        float dist = Vector3.Distance(transform.position, player.transform.position);

        

        if (!agent.pathPending)
        {
            
            // TODO - do we have line of sight? if we dont have line of sight, keep patrolling, if we do have line of sight, skip to the second step

            if (dist > ChaseDist || !PlayerVisible())
            {
                Target = null;
                // TODO using the POI system, if target is not in range, select a random point and traverse to that point
                // very far away, keep patrolling
                Debug.DrawRay(transform.position, agent.destination - transform.position, Color.yellow);

                if (agent.remainingDistance < 0.1f || (agent.velocity.magnitude < 1.0f && Time.time >= NavRescanAfter))
                    GotoNextPoint();
            }
            else if (dist < MinApproach) // Too close, back off
            {
                Target = player.transform;

                TargetPlayer(ChaseDist);

            }
            else // Target in potential-visible range, but outside "Min" distance so should get closer!
            {
                // move closer to player
                agent.SetDestination(player.transform.position);

                //  Target = player.transform;
                TargetPlayer(ChaseDist);
            }


        }
    }

    void SetDestination(Vector3 pt)
    {

    }


    const int Structure05mMask = 1 << 9; // See Unity Layers for "Structure 05m"
    const int Structure10mMask = 1 << 10; // See Unity Layers for "Structure 10m"


    public bool PlayerVisible()
    {

        // Define Raycast mask:  Always ignore 5m buildings, and also ignore 10m buildings when Character Level 3 or greater
        var layerMask = ~(Structure05mMask | (player.GetComponent<Kataklizma.Gameplay.EntityAttributes>()[ValueType.CharacterLevel] > 1 ? Structure10mMask : 0));


        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, ChaseDist, layerMask ))
        {
            return (hit.collider.gameObject.tag == "Player");
        }

        return false;
    }

    void TargetPlayer(float ChaseDist)
    {
        // Define Raycast mask:  Always ignore 5m buildings, and also ignore 10m buildings when Character Level 3 or greater
        var layerMask = ~(Structure05mMask | (player.GetComponent<Kataklizma.Gameplay.EntityAttributes>()[ValueType.CharacterLevel] > 1 ? Structure10mMask : 0));

        RaycastHit hit;

        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, ChaseDist, layerMask))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                Target = hit.collider.gameObject.transform;
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);

                NavRescanAfter = Time.time + NavTimeout;
            }
        }
    }
}