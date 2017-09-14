using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlades : MonoBehaviour
{
    public int speed = 0;
    void Update()
    {
        transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
    }
}