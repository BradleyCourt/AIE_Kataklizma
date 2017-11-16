using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {
    private Vector3 trackedOrigin;
    public float Displacement = 1.0f;

    const float Frequency = 0.05f;

	// Use this for initialization
	void Start () {
        trackedOrigin = transform.position;
        Tween();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void Tween()
    {
        //LeanTween.moveX(gameObject, 1, 0.2f);
        var current = new Vector2(transform.position.x, transform.position.z);
        var target = new Vector2(trackedOrigin.x , trackedOrigin.z) + Random.insideUnitCircle * Displacement;

        LeanTween.value(gameObject, (value) =>
        {

            var posn = transform.position;
            posn.x = value.x;
            posn.z = value.y;

            transform.position = posn;

        }, current, target, Frequency)
        .setOnComplete(() => { Invoke("Tween", 0); });
    }
}
