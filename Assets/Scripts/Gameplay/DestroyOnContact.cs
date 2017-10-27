using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    { 
            Destroy(gameObject);
    }
}
