using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Must Have a CharacterController
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]


public class Movement_Adrian : MonoBehaviour {

    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_GroundCheckDistance = 0.1f;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_JumpPower = 12f;
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;

    //Gets Animator
    Animator animator;
    //Gets Character Controller
    CharacterController CharControler;
    //Gets Players Speed
    public float m_speed;
    //Rotation Speed
    public float m_rotationSpeed;
    //Velocity
    protected Vector3 velocity;
    //Rock Count
    public int rockCount;
    //Forward ammount
    float m_ForwardAmount;
    //Turning amount
    float m_TurnAmount;
    //Check If Grounded
     public bool m_IsGrounded;
    //Ground Check Distance
    float m_OrigGroundCheckDistance;
    
    const float k_Half = 0.5f;
   //Normal
    Vector3 m_GroundNormal;
    //Cap Height
    float m_CapsuleHeight;
    //Cap Center
    Vector3 m_CapsuleCenter;
    //Crouching
    bool m_Crouching;
    


    // Use this for initialization
    void Start () {

        //Gets Animator
        animator = GetComponent<Animator>();
        //Gets character controller Component
        CharControler = GetComponent<CharacterController>();
        m_CapsuleHeight = CharControler.height;
        m_CapsuleCenter = CharControler.center;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
        m_JumpPower = 10;
    }

  

    public void Move(Vector3 move, bool crouch, bool jump)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (m_IsGrounded)
        {
            HandleGroundedMovement(crouch, jump);
        }
        else
        {
            HandleAirborneMovement();
        }

        //Gravity Math
        m_JumpPower -= 9.8f * Time.deltaTime;

        if (CharControler.isGrounded)
        {
            m_JumpPower = 0;
        }

        ScaleCapsuleForCrouching(crouch);
        PreventStandingInLowHeadroom();

        // send input and other state parameters to the animator
        UpdateAnimator(move);
     }
	
    //OLD UPDATE CODE

	//// Update is called once per frame
	//void Update () {

 //       //Sets the cursor locked and cannot not be seen
 //       Cursor.lockState = CursorLockMode.Locked;
 //       Cursor.lockState = CursorLockMode.None;
 //       Cursor.visible = false;

 //       if (CharControler.isGrounded)
 //       {
 //           moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
 //           moveDirection = transform.TransformDirection(moveDirection);
 //           moveDirection *= m_speed;

 //       }
 //       moveDirection.y -= m_Gravity * Time.deltaTime;
 //       CharControler.Move(moveDirection * Time.deltaTime);

 //       transform.Rotate(0, Input.GetAxis("Horizontal"), 0);


 //       //Forward movement Animation
 //       if (Input.GetKeyDown(KeyCode.W))
 //       {
 //           animator.SetBool("IsMovingForward", true);
 //       }
 //       else if (Input.GetKeyUp(KeyCode.W))
 //       {
 //           animator.SetBool("IsMovingForward", false);
 //       }


 //       //Grounded checks
 //       if (CharControler.isGrounded)
 //       {
 //           Debug.Log("Grounded");
 //           m_speed = 10;
 //       }
 //       else
 //       {
 //           Debug.Log("Not Grounded");
 //           m_speed = 0;
 //       }

 //   }


    void ScaleCapsuleForCrouching(bool crouch)
    {
        if (m_IsGrounded && crouch)
        {
            if (m_Crouching) return;
            CharControler.height = CharControler.height / 2f;
            CharControler.center = CharControler.center / 2f;
            m_Crouching = true;
        }
        else
        {
            Ray crouchRay = new Ray(CharControler.transform.position + Vector3.up * CharControler.radius * k_Half, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - CharControler.radius * k_Half;
            if (Physics.SphereCast(crouchRay, CharControler.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_Crouching = true;
                return;
            }
            CharControler.height = m_CapsuleHeight;
            CharControler.center = m_CapsuleCenter;
            m_Crouching = false;
        }
    }

    void ApplyExtraTurnRotation()
    {
        float turnspeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnspeed * Time.deltaTime, 0);
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        animator.SetBool("Crouch", m_Crouching);
        animator.SetBool("OnGround", m_IsGrounded);
        if (!m_IsGrounded)
        {
            animator.SetFloat("Jump", CharControler.velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded)
        {
            animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (m_IsGrounded && move.magnitude > 0)
        {
            animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            animator.speed = 1;
        }
    }

    void PreventStandingInLowHeadroom()
    {
        // prevent standing up in crouch-only zones
        if (!m_Crouching)
        {
            Ray crouchRay = new Ray(CharControler.transform.position + Vector3.up * CharControler.radius * k_Half, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - CharControler.radius * k_Half;
            if (Physics.SphereCast(crouchRay, CharControler.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_Crouching = true;
            }
        }
    }
    

    //Getters and Setters For Rock Count
    public int GetRockCount() { return rockCount; }
    public void SubtractRockCount() { rockCount--; }

    //OnTriggerStay For Rock Pickup
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            Debug.Log("Rock");

            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("E");

                other.gameObject.SetActive(false);
                Destroy(other.gameObject);
                rockCount++;
            }
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        CharControler.SimpleMove(extraGravityForce); 
    }


    void HandleGroundedMovement(bool crouch, bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !crouch && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump!
            m_JumpPower += 100 * Time.deltaTime;
            transform.Translate(new Vector3(0, m_JumpPower, 0));
            m_IsGrounded = false;
            animator.applyRootMotion = false;
        }
    }

    void CheckGroundStatus()
    {
        if (CharControler.isGrounded)
        {
            m_IsGrounded = true;
            animator.applyRootMotion = true;
        }
        else
        {
            m_IsGrounded = false;  
            animator.applyRootMotion = false;
        }
    }
}
