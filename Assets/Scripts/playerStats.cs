using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[DisallowMultipleComponent]
public class playerStats : MonoBehaviour
{
    public int MaxHealth;
    private int _CurrentHealth;
    public Slider HealthBar;
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
        HealthBar.maxValue = MaxHealth;
        HealthBar.minValue = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void RemoveHealth(int Damage)
    {
        CurrentHealth -= Damage;
        HealthBar.value = CurrentHealth;
    }

}

