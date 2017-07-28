using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObserverDirector))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {


    enum AnimState {
        Idle,
        Moving,
        Jumping,
        Attacking,
        Charging,
    }

    private AnimState State = AnimState.Idle;

    public float Speed = 5;
    public float ChargeSpeed = 7.5f;

    //public float ChargeLimit = 1; // Maximum number of allowed charges
    public float ChargeCooldown = 5; // Time before next charge is available
    public float ChargeDuration = 1.8f; // Time charge lasts for if not interrupted
    private bool IsChargeReady = true;
    private bool CanCharge {
        get {
            return IsChargeReady && Cc.isGrounded && (State == AnimState.Idle || State == AnimState.Moving);
        }
    }


    public float MeleeAttackCooldown = 1;
    public float MeleeAttackDuration = 0.3f;
    private bool IsMeleeAttackReady = true;
    private bool CanAttack {
        get {
            return IsMeleeAttackReady;
        }
    }

    //private int ChargeCount = 0;

    
    public float JumpVel = 8.0F;
    public float JumpCooldown = 0;
    public float JumpDuration = 0.2f;
    private bool CanJump = true;

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

        // Process input
        if (IsControllable) {
            if (CanCharge && Input.GetButtonDown("Charge")) {
                DoCharge();
            }

            if (CanAttack && Input.GetButtonDown("Fire1"))
                DoMeleeAttack();

            if (Cc.isGrounded && CanJump && Input.GetButtonDown("Jump"))
                DoJump();
            

            


        }

        // Move character
        Vector3 motion;
        if (State != AnimState.Charging) {
            motion = GetPlayerMotion() * ChargeSpeed;
            Observers.Target.transform.LookAt(Observers.Target.transform.position + motion);
        } else {
            motion = transform.forward * ChargeSpeed;
        }
        
        Cc.SimpleMove(motion);


        /////////////////
        if (IsControllable) {


            if (Cc.isGrounded) {

                if (Input.GetButtonDown("Jump")) {
                    moveDirection.y = JumpVel;
                }
            } else {

                //moveDirection += Physics.gravity * Time.deltaTime;
            }
        } else {
            // there needs to be a check where if the player is charging, if it collides with an object it stops charging, but if it collides with an enemy, it does damage

        }

        //Cc.Move(moveDirection * Time.deltaTime);
    }


    /// <summary>
    /// Apply captured motion to character.  Update character velocity.
    /// </summary>
    Vector3 GetPlayerMotion() {

        if (!IsControllable) return Vector3.zero; // Player-control disabled

        var cameraDirection = Observers.Target.transform.position - Observers.Selected.Observer.transform.position;

        var characterFwd = new Vector3(cameraDirection.x, 0, cameraDirection.z).normalized;
        var characterRight = new Vector3(cameraDirection.z, 0, -cameraDirection.x).normalized;

        var motion = characterFwd * Input.GetAxis("MoveVertical") + characterRight * Input.GetAxis("MoveHorizontal");
        motion.Normalize();

        

        return motion;
    }


    /// <summary>
    /// 
    /// </summary>
    void DoCharge() {
        IsControllable = false;
        IsChargeReady = false;
        State = AnimState.Charging;

        // Setup delay for returning control
        StartCoroutine(this.DelayedAction(ChargeDuration,
            () => {
                if (State == AnimState.Charging) { // If we're still 'charging' (ie, if we haven't hit anything already)
                    State = AnimState.Idle;
                    IsControllable = true;
                }
            }));

        // Setup delay for cooldown
        StartCoroutine(this.DelayedAction(ChargeCooldown,
            () => {
                IsChargeReady = true;
            }));

        // Do Charge
    }


    /// <summary>
    /// 
    /// </summary>
    void DoJump() {

    }


    /// <summary>
    /// 
    /// </summary>
    void DoMeleeAttack() {
        State = AnimState.Attacking;
        IsMeleeAttackReady = false;

        // Setup delay for returning control
        StartCoroutine(this.DelayedAction(MeleeAttackDuration,
            () => {
                if (State == AnimState.Attacking) {
                    State = AnimState.Idle;
                }
            }));

        // Setup delay for cooldown
        StartCoroutine(this.DelayedAction(MeleeAttackCooldown,
            () => {
                IsMeleeAttackReady = true;
            }));



        // Do Attack
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
            State = AnimState.Idle;
            Debug.Log("hit!");
        }
    }

    private void OnCollisionEnter(Collision collision) {
        //IsCharging = false;
    }
}
