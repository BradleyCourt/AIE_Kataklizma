using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 5;
    public float jumpVel = 8.0F;
    public CharacterController cc;
 //   private Vector3 moveDirection = Vector3.zero;

    public Vector3 moveDirection;

    // Use this for initialization
    void Start ()
    {
       cc = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal")*speed, moveDirection.y, Input.GetAxisRaw("Vertical")*speed);


        // Gravity at current does not work!  
        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpVel;
            }
        }
        else
        {
            moveDirection += Physics.gravity * Time.deltaTime;
        }

        cc.Move(moveDirection* Time.deltaTime);
    }
   
}
