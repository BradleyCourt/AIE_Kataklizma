using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Obsolete]
public class HeliChase : MonoBehaviour
{
    public Transform Target;
    private GameObject player;
    private List<Transform> points = new List<Transform>(); // TODO Use POI to get the patrol locations
    private NavMeshAgent agent;
    public int ChaseDist = 200;
 

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.isOnNavMesh) return;

        //TODO make the Y vector always 0
        float dist = Vector3.Distance(transform.position, player.transform.position);
        // TODO - do we have line of sight? if we dont have line of sight, keep patrolling, if we do have line of sight, skip to the second step

        if (dist > ChaseDist)
        {
            Target = null;
            // TODO using the POI system, if target is not in range, select a random point and traverse to that point
            // very far away, keep patrolling
            Debug.DrawRay(transform.position, agent.destination - transform.position, Color.yellow);
        }
        else 
        {
            // move closer to player
            agent.SetDestination(player.transform.position);
            //  Target = player.transform;
            TargetPlayer(ChaseDist);
        }
    }

        // Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.cyan);
    

    void TargetPlayer(float ChaseDist)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, ChaseDist))
        {
            //Debug.Log(gameObject.name + " - HeliChase::TargetPlayer(): Hit [" + hit.collider.gameObject.tag + "]");
            if (hit.collider.gameObject.tag == "Player")
            {
                Target = hit.collider.gameObject.transform;
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);

            }
        }
    }
}
