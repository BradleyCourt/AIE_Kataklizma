using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int speed = 5;
    public float jumpVel = 8.0F;
    public CharacterController cc;
    public GameObject Player;
    Animator animate;
    //   private Vector3 moveDirection = Vector3.zero;

    private Vector3 moveDirection;

    BoxCollider attackZone;

    // Use this for initialization
    void Start() {
        cc = GetComponent<CharacterController>();
        animate = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {



        moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, moveDirection.y, Input.GetAxis("Vertical") * speed);


        // Gravity at current does not work!  
        if (cc.isGrounded) {

            if (Input.GetButtonDown("Jump")) {
                moveDirection.y = jumpVel;
            }
        } else {

            moveDirection += Physics.gravity * Time.deltaTime;
        }

        cc.Move(moveDirection * Time.deltaTime);

        //if (Input.GetMouseButtonDown(0)) {
        //    if (!animate.IsInTransition(0)) {
        //        animate.SetBool("Attacking", true);
        //        Debug.Log("Hah!");
        //    }
        //}
    }

    void Attack() {
        Collider[] targets = Physics.OverlapBox(attackZone.bounds.center + transform.position /* ?? */, attackZone.bounds.extents, transform.rotation);
        foreach (Collider target in targets) {
            var playerStats = target.GetComponent<playerStats>();
            if (playerStats) {
                // Apply Damage
            }

        }

    }

}
