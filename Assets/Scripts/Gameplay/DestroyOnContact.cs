using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    public GameObject explosion;
    protected bool runOnce = true;

    void OnCollisionEnter(Collision collision)
    {
        if (explosion != null && runOnce)
        {
            runOnce = false;
            var go = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(go, 2);
        }

        Destroy(gameObject);
    }
}
