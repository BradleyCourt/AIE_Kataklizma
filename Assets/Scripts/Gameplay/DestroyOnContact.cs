using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    public GameObject explosion;
    private bool check = true;
    void OnCollisionEnter(Collision collision)
    { 
        if(explosion != null && check != false)
        {
            Transform test = gameObject.transform;
            Instantiate(explosion, test);
            print("explosion");
            //Destroy(explosion, 2);
            check = false;
        }
            //Destroy(gameObject);
    }
}
