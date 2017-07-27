using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseOnDeath : MonoBehaviour
{
    private bool Falling { get; set; }


    public float fallTime = 1;
    public float fallSpeed = 0.05f;

    // Use this for initialization
    void Start ()
    {
        Falling = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Falling == true)
        {
            fallTime -= Time.deltaTime;
            transform.position -= new Vector3(0, fallSpeed, 0);
            if (fallTime <= 0)
            {
                Debug.Log("remove me");
                Destroy(gameObject);
            }
        }
    }

    void OnObjectDeath()
    { 
        Falling = true;
        //Destroy(gameObject);
    }

}
