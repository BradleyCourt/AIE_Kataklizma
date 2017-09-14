using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeliChase : MonoBehaviour
{
    public Transform Target;
    private GameObject player;
    private List<Transform> points = new List<Transform>(); // TODO Use POI to get the patrol locations
    private NavMeshAgent agent;

    // Use this for initialization
    void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.cyan);
	}

    void TargetPlayer(float ChaseDist)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, ChaseDist))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Target = hit.collider.gameObject.transform;
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);

            }
        }
    }
}
