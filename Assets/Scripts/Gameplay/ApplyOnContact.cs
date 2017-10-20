using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyOnContact : MonoBehaviour
{
    public List<ValueCollection.Value> Effects;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<EntityAttributes>() != null)
       collision.gameObject.GetComponent<EntityAttributes>().ApplyEffects(Effects);
    }
}
