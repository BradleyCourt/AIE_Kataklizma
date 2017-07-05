using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int speed = 5;
    public float jumpHeight = 8.0F;
    public float gravity = 9.8f;
    private Vector3 moveDirection = Vector3.zero;
    public bool isGrounded = true;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        moveDirection = transform.localPosition += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
    
        // Gravity at current does not work!
        if (isGrounded == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isGrounded = false;
                moveDirection.y = jumpHeight;
                moveDirection.y -= gravity * Time.deltaTime;
            }
            isGrounded = true;
        }
    }
}
