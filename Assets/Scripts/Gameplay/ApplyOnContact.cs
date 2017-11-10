using Kataklizma.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityAttributes))]
public class ApplyOnContact : MonoBehaviour
{
    [UnityTag]
    public List<string> CanAffectTags;

    public List<ValueType> EffectsToApply;

    protected EntityAttributes _Attributes;
    protected EntityAttributes Attributes {
        get {
            if (_Attributes == null) {
                _Attributes = GetComponent<EntityAttributes>();
                if (_Attributes == null)
                    Debug.LogError(gameObject.name + " - ApplyOnContact: Unable to locate required EntityAttributes sibling");
            }

            return _Attributes;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!CanAffectTags.Contains(collision.gameObject.tag)) return; // Collided with invalid object

        var otherAttributes = collision.gameObject.GetComponent<EntityAttributes>();

        if (otherAttributes == null || Attributes == null) return; // Non-affective object

        
        otherAttributes.ApplyEffects(Attributes.GetEffects(EffectsToApply));

    }
}
