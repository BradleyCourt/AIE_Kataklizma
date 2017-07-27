using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class playerStats : MonoBehaviour
{
    public int MaxHealth;

    private int _CurrentHealth;
    public int CurrentHealth {
        get { return _CurrentHealth; }
        set {

            _CurrentHealth = Mathf.Max(value, 0);

            if (_CurrentHealth <= 0)
            {

                BroadcastMessage("OnObjectDeath", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Object Died: " + gameObject.name);
            }
        }
    }

    public bool IsAlive {  get { return _CurrentHealth > 0; } }


	// Use this for initialization
	void Start ()
    {
        CurrentHealth = MaxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {

	}


    public void RemoveHealth(int Damage) {
        CurrentHealth -= Damage;
    }

}

