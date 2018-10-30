using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Must Have a CharacterController
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]


public class Movement_Adrian : MonoBehaviour
{

    private float m_StationaryTurnSpeed = 180;
    private float m_MovingTurnSpeed = 500;
    private float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [Range(0.1f, 4f)] [SerializeField] float m_WalkSpeed = .8f;
    [Range(0.1f, 4f)] [SerializeField] float m_SprintSpeed = 1.2f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;

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
    bool m_IsGrounded;
    //Sprint Timer
    float m_fSprintTimer;
    //Sprint Duration
    public float m_fSprintDuration = 5.0f;
    //Time till run again
    public float m_fRunAgainTime = 2.5f;
    //Ready to sprint bool used for check
    bool m_bReadyToSprint;
    //Map Image
    public Image Map;
    //Map Open Bool
    bool MapOpen = false;
    //Bool Stop for sound;
    bool StoppedForSound = false;

    const float k_Half = 0.5f;
    //Cap Height
    float m_CapsuleHeight;
    //Cap Center
    Vector3 m_CapsuleCenter;
    //Crouching
    bool m_Crouching;
    // QTevent started
    [HideInInspector]
    public bool m_bQuicktime;


    private bool aiming;

    public Text RockPickUpUi;
    public Text AimRockUI;
    public Text ThrowRockUI;

    public GameObject spawnpoint = null;

    // Use this for initialization
    void Start()
    {
        //Gets Animator
        animator = GetComponent<Animator>();
        //Gets character controller Component
        CharControler = GetComponent<CharacterController>();
        m_CapsuleHeight = CharControler.height;
        m_CapsuleCenter = CharControler.center;
        m_Crouching = false;
        //Set is aiming
        aiming = false;
        //Set Quicktime
        m_bQuicktime = false;
        //Set timer to current duration sprint
        m_fSprintTimer = m_fSprintDuration;
        //Set Ready to sprint
        m_bReadyToSprint = true;
        //Sets Rock Ui
        RockPickUpUi.enabled = false;
        //Sets Aim Ui
        AimRockUI.enabled = false;
        //Sets Throw Rock Ui
        ThrowRockUI.enabled = false;
    }

    public void Move(Vector3 move, bool crouch, bool jump, bool sprinting)
    {
        if (rockCount > 0)
        {
            if (AimRockUI != null && ThrowRockUI != null)
            {
                if (Input.GetMouseButton(0))
                {
                    ThrowRockUI.enabled = true;
                    AimRockUI.enabled = false;
                }

                else
                {
                    ThrowRockUI.enabled = false;
                    AimRockUI.enabled = true;
                }
            }
        }

        else
        {
            if (AimRockUI != null)
            {
                ThrowRockUI.enabled = false;
                AimRockUI.enabled = false;
            }
        }

        //If not aiming and not in a quicktime event
        if (!aiming && !m_bQuicktime && !StoppedForSound)
        {
            if (move.magnitude > 1f)
                move.Normalize();

            move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            m_TurnAmount = Mathf.Atan2(move.x, move.z);

            //If Button Press and bool True Sprint
            if (sprinting && m_bReadyToSprint)
            {
                //Adds Sprinting Speed
                m_ForwardAmount = move.z * m_SprintSpeed;
                //Starts Sprint Timer Reducing time
                m_fSprintTimer -= Time.deltaTime;

                //Sets Bool To False if condition is met
                if (m_fSprintTimer <= 0)
                {
                    m_bReadyToSprint = false;
                }

                //if timer hits 0 reset
                if (m_fSprintTimer <= 0)
                {
                    m_fSprintTimer = 0;
                }
            }
            else
            {
                //Adds Walking Speed
                m_ForwardAmount = move.z * m_WalkSpeed;
                //Starts Sprint Timer Adding time
                m_fSprintTimer += Time.deltaTime;

                //Sets Bool To true if condition is met
                if (m_fSprintTimer >= m_fRunAgainTime)
                {
                    m_bReadyToSprint = true;
                }

                //Resets timer if out of Paramaters
                if (m_fSprintTimer >= m_fSprintDuration)
                {
                    m_fSprintTimer = m_fSprintDuration;
                }
            }

            ApplyExtraTurnRotation();

            // control and velocity handling is different when grounded and airborne:
            if (m_IsGrounded)
            {
                HandleGroundedMovement(crouch, jump);
                HandleAirborneMovement();
            }
            else
            {
                HandleAirborneMovement();
            }

            //Crouching Capsuale adjustments
            ScaleCapsuleForCrouching(crouch);

            // send input and other state parameters to the animator
            UpdateAnimator(move);
            if (spawnpoint != null)
            {
                spawnpoint.GetComponent<Trajectory_Simulation>().enabled = false;
                spawnpoint.GetComponent<LineRenderer>().enabled = false;
            }
        }
        else
        {
            ApplyExtraTurnRotation();

            m_ForwardAmount = 0;
            m_rotationSpeed = 0;
            m_TurnAmount = 0;


            Debug.Log("Stopped moving");
            UpdateAnimator(move);

            //ADD ARC HERE
            if (spawnpoint != null && aiming)
            {
                spawnpoint.GetComponent<Trajectory_Simulation>().enabled = true;
                spawnpoint.GetComponent<LineRenderer>().enabled = true;

                Quaternion charRot = Camera.main.transform.rotation;
                charRot.x = 0;
                charRot.z = 0;
                transform.rotation = charRot;
            }

        }
    }

    void ScaleCapsuleForCrouching(bool crouch)
    {
        if (crouch && m_IsGrounded)
        {
            CharControler.center = new Vector3(0, 0.55f, 0);
            CharControler.height = 1.0f;
            m_Crouching = true;
            m_WalkSpeed = 1.2f;

        }
        else
        {
            CharControler.center = m_CapsuleCenter;
            CharControler.height = m_CapsuleHeight;
            m_Crouching = false;
            m_WalkSpeed = 0.6f;
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


    //Getters and Setters For Rock Count
    public int GetRockCount() { return rockCount; }
    public void SubtractRockCount() { rockCount--; }

    //OnTriggerStay For Rock Pickup
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            Debug.Log("Rock");

            if (Input.GetKey(KeyCode.E) && other.GetComponent<Rock_Adrian>().canPickup)
            {
                Debug.Log("E");

                other.gameObject.SetActive(false);
                Destroy(other.gameObject);
                rockCount++;
                if (RockPickUpUi != null)
                {
                    RockPickUpUi.enabled = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rock") && RockPickUpUi != null && other.GetComponent<Rock_Adrian>().canPickup)
        {
            RockPickUpUi.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Rock") && RockPickUpUi != null)
        {
            RockPickUpUi.enabled = false;
        }
    }


    void HandleAirborneMovement()
    {
        Vector3 velocity;
        velocity = CharControler.velocity;
        velocity.y -= 9.8f * Time.deltaTime;
        CharControler.Move(velocity * Time.deltaTime);
    }


    void HandleGroundedMovement(bool crouch, bool jump)
    {
        // check whether conditions are right to allow a jump:
        //if (jump && !crouch && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
       // {
         //   CharControler.transform.Translate(new Vector3(0, 1, 0));
       // }
    }

    void CheckGroundStatus()
    {
        RaycastHit raycastHit;
        Debug.DrawRay(transform.position + (Vector3.up * 0.2f), Vector3.down);
        if (m_Crouching)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out raycastHit, 0.6f))
            {
                Debug.Log("Grounded");
                m_IsGrounded = true;
                animator.applyRootMotion = true;
            }
            else
            {
                Debug.Log("Not Grounded");
                m_IsGrounded = false;
                animator.applyRootMotion = false;
            }
        }
        else
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out raycastHit, 0.6f))
            {
                Debug.Log("Grounded");
                m_IsGrounded = true;
                animator.applyRootMotion = true;
            }
            else
            {
                Debug.Log("Not Grounded");
                m_IsGrounded = false;
                animator.applyRootMotion = false;
            }
        }

    }

    public void stopCharacter()
    {
        aiming = true;
    }
    //Stop movement when sound is playing
    public void SoundStop()
    {
        StoppedForSound = true;
    }
    //Start movement when sound is playing
    public void SoundStart()
    {
        StoppedForSound = false;
    }

    public void rockThrown()
    {
        aiming = false;
    }

    public void SetQuicktime(bool IsStopped)
    {
        m_bQuicktime = IsStopped;
    }

    public void SetForwardAmount(float amount)
    {
        m_ForwardAmount = amount;
    }

    public void MapToggle(bool Mbutton)
    {
        //Input checks
        if (Mbutton)
        {
            if (MapOpen)
            {
                MapOpen = false;
            }
            else
            {
                MapOpen = true;
            }
        }

        //Opening And Closing Image
        if (MapOpen)
        {
            Map.enabled = true;
        }
        else
        {
            Map.enabled = false;
        }
    }
}