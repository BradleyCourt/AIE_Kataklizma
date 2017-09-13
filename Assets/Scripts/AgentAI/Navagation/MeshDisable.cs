using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDisable : MonoBehaviour
{
    public Vector3 Size;
    //public GameObject plane;
    // Use this for initialization
    void Start ()
    {
        //NavMeshGen.BuildNavMesh(transform, new Bounds(transform.position, Size));

        this.GetComponent<MeshRenderer>().enabled = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
