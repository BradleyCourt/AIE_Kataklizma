using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDisable : MonoBehaviour
{
    public Vector3 Size;
    //public GameObject plane;
    // Use this for initialization
    bool firstUpdate;

    void Start ()
    {
        //NavMeshGen.BuildNavMesh(transform, new Bounds(transform.position, Size));

        firstUpdate = true;


    }
	
	// Update is called once per frame
	void Update () {

        if (firstUpdate)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            firstUpdate = false;
        }	
	}
}
