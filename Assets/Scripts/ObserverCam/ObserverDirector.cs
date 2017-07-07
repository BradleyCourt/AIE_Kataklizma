using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverDirector : MonoBehaviour {

    [System.Serializable]
    public class ObserverDefinition {
        public ObserverController Observer;
        public float MinPitch = 30;
        public float MaxPitch = 80;
        public float MinRange1 = 10;
        public float MaxRange1 = 30;
    }

    public GameObject Target;
    protected ObserverDefinition Selected;

    public List<ObserverDefinition> Observers;

	// Use this for initialization
	void Start () {
        if (Observers == null || Observers.Count < 1)
            throw new System.InvalidOperationException("ObserverDirector REQUIRES at least one Observer Definition");

        Selected = Observers[Random.Range(0, Observers.Count)];
	}
	
	// Update is called once per frame
	void Update () {

        var view = new Vector3();
        view.x = Input.GetAxis("CameraHorizontal");
        view.y = Input.GetAxis("CameraVertical");
        view.z = Input.GetAxis("CameraZoom");

        var currentOffset = Selected.Observer.transform.position - Target.transform.position;
        var distance = currentOffset.magnitude;
        var direction = currentOffset.normalized;

        var yaw = Quaternion.Euler(new Vector3(0, Input.GetAxis("CameraHorizontal"), 0));
        var pitch = Quaternion.Euler(new Vector3(Input.GetAxis("CameraVertical"), 0, 0));
        var zoom = distance * Mathf.Clamp((1.0f + Input.GetAxis("CameraZoom")), 0.8f, 1.2f);

        
        if (distance <= 1.0f)
            zoom = Mathf.Max(1.0f, zoom);

        var offset = direction;
        offset = yaw * offset;
        offset = pitch * offset;
        offset = zoom * offset;

        // Rotate camera around target
        Selected.Observer.transform.position = (Target.transform.position + offset);

      

    }
}
