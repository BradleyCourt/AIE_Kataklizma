using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDisable : MonoBehaviour {

    void Update() {
        this.GetComponent<MeshRenderer>().enabled = false;
        Destroy(this);
    }
}
