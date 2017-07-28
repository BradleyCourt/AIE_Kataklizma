using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObserverDirector))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    public int Speed = 5;
    public float ChargeLimit = 1;
    public int charge = 50; // not implemented yet, intention is once used movement is limited and you charge for X seconds or until collision with something
    public float JumpVel = 8.0F;
    public int TempDamage = 0;

    public bool IsControllable { get; set; }
    //   private Vector3 moveDirection = Vector3.zero;

    private Vector3 moveDirection;

    public BoxCollider attackZone; // melee attack
    public CapsuleCollider fireBreath; // fire breath attack

    private ObserverDirector Observers;
    private CharacterController Cc;

    // Use this for initialization
    void Start() {
        Observers = GetComponent<ObserverDirector>();
        if (Observers == null) throw new System.ApplicationException("PlayerController requires an ObserverDirector sibling");

        Cc = GetComponent<CharacterController>();
        if (Cc == null) throw new System.ApplicationException("PlayerController requires a CharacterController sibling");

        IsControllable = true;
    }


    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Charge")) {
            if (!IsControllable) {
                ChargeLimit = 1;
            }
            IsControllable = false;
            
        }

        UpdateMotion();

        if (IsControllable) {
            if (Input.GetMouseButtonDown(0))
                MeleeAttack();

            if (Cc.isGrounded) {

                if (Input.GetButtonDown("Jump")) {
                    moveDirection.y = JumpVel;
                }
            } else {

                moveDirection += Physics.gravity * Time.deltaTime;
            }
        } else {
            // there needs to be a check where if the player is charging, if it collides with an object it stops charging, but if it collides with an enemy, it does damage
            ChargeLimit -= Time.deltaTime;
            if (ChargeLimit >= 0) {
                moveDirection = transform.forward;
            } else {
                IsControllable = true;
                ChargeLimit = 1;
            }
        }

        Cc.Move(moveDirection * Time.deltaTime);
    }


    /// <summary>
    /// Apply captured motion to character.  Update character velocity.
    /// </summary>
    void UpdateMotion() {

        if (!IsControllable) return; // Player-control disabled

        var cameraDirection = Observers.Target.transform.position - Observers.Selected.Observer.transform.position;

        var characterFwd = new Vector3(cameraDirection.x, 0, cameraDirection.z).normalized;
        var characterRight = new Vector3(cameraDirection.z, 0, -cameraDirection.x).normalized;

        var motion = characterFwd * Input.GetAxis("MoveVertical") + characterRight * Input.GetAxis("MoveHorizontal");
        motion.Normalize();

        Observers.Target.transform.LookAt(Observers.Target.transform.position + motion);
        //Observers.TargetRb.velocity = motion * 5.0f;

        /// NOTE: Would really like to remove CharacterController dependency.
        Cc.SimpleMove(motion * 5.0f);
    }


    void MeleeAttack() {

        var centreOffset = transform.localToWorldMatrix.MultiplyVector(attackZone.center);
        var centre = transform.position + centreOffset;
        var size = transform.localToWorldMatrix.MultiplyVector(attackZone.size);

        Collider[] targets = Physics.OverlapBox(centre, size, new Quaternion(), ~8);

        foreach (Collider target in targets) {
            if (target.gameObject.tag == "Player") continue;

            var playerStats = target.GetComponent<playerStats>();
            if (playerStats) {
                playerStats.RemoveHealth(TempDamage);
            }

        }

    }

    void FireAttack() // mainly a cut and paste from the function above, untested ( shouuuuuld work, just has to be called from the attack  )
    {

        var centreOffset = transform.localToWorldMatrix.MultiplyVector(attackZone.center);
        var centre = transform.position + centreOffset;
        var size = transform.localToWorldMatrix.MultiplyVector(attackZone.size);

        Collider[] targets = Physics.OverlapCapsule(centre, size, ~8);

        foreach (Collider target in targets) {
            if (target.gameObject.tag == "Player") continue;

            var playerStats = target.GetComponent<playerStats>();
            if (playerStats) {
                playerStats.RemoveHealth(TempDamage);
            }

        }

    }

    void OnTriggerEnter(Collider col) {

        if (col.gameObject.tag == "Enemy") {
            ChargeLimit = 0;
            Debug.Log("hit!");
        }
    }


}
