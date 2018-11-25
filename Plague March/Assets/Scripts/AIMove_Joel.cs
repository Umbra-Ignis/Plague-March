//========================================================================================
//AIMove_Joel
//
//Functionality: Used to control the AI within the game, the transitions between states
//and the behaviour within states
//
//Author: Joel G
//Altered by: Adrian P
//========================================================================================
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIMove_Joel : MonoBehaviour
{
    //Takes in an array of waypoints for the AI to patrol between
    public Transform[] targets;

    //Determines Speed
    [Range(1f, 50f)] public float m_Speed = 1f;
    //Distance agent Will stop Relative to waypoint
    [Range(1f, 50f)] public float stoppingDistance;
    //Sets how much faster the chasing state will be, 1 being the same speed as patrol
    [Range(1f, 20f)] public float chaseSpeedMultiplier;
    //Time Before Start Chasing
    [Range(1f, 10f)] public float m_fChaseWaitTime;

    //Used to time how long is spent at the current waypoint, once arrived
    public float waypointWaitTime;
    //Determines whether the AI loops around the list of waypoints, or cycles through one way, then back
    public bool loop;
    //Current Distance the agent is away from the Waypoint
    private float distanceToWaypoint;

    //Stores a reference to the player, making their position easily obtainable
    [HideInInspector] public GameObject player;
    //Obtains reference to the agent to set its targets etc
    [HideInInspector] public NavMeshAgent agent;


    //Stores whether the AI is patrolling
    private bool patrolling;
    //Stores whether the AI is alerted
    private bool alerted;
    //Stores whether the AI is chasing
    private bool chasing;
    //Stores whether the AI is approaching a thrown rock
    private bool rock;
    //Wait Timer Patrol
    private float timerPatrol;
    //Wait Timer Alert
    private float timerAlert;
    //Rock wait time
    public float m_rockWaitTime;
    //Rock wait timer
    private float m_rockWaitTimer;

    //Stores a reference to the current target of the AI
    [HideInInspector]
    public Vector3 currentTarg;

    //Stores the animator of the actor in which needs to be altered
    [HideInInspector]
    public Animator anim;

    //Used to store the int of the current target
    private int targVal;

    //Used to store whether the AI is progressing forward or backward through the waypoints
    private bool forward;

    //Takes reference to the open eye sprite to apear in the UI, depending on the state of the AI
    public Image Open = null;
    //Takes reference to the half open eye sprite to apear in the UI, depending on the state of the AI
    public Image Half = null;
    //Takes reference to the red open eye sprite to apear in the UI, depending on the state of the AI
    public Image Red = null;

    //Stores whether or not the AI only has one waypoint or not
    private bool oneWaypoint;

    //Takes a reference to the starting waypoint of the AI
    public Transform startWaypoint;
    //Stores a reference to the AI's character controller
    private CharacterController charCont;
    //Stores a reference to the AI's capsule collider
    private CapsuleCollider infectedCapsule;

    //Stores whether or not the quick time event is currently active
    private bool QTE;

    //Takes reference to the AI's infection centrepoint
    public GameObject infectionCP;

    // Use this for initialization
    void Start()
    {
        //Gets the NavMesh agent component from the AI to set waypoints and such
        agent = GetComponent<NavMeshAgent>();
        //Gets the Animator component from the AI to alter the animations
        anim = GetComponent<Animator>();
        //Gets the character controller component
        charCont = GetComponent<CharacterController>();
        infectedCapsule = GetComponent<CapsuleCollider>();

        //Sets the AI's default behaviour to patrol
        SetPatrol();

        //Ensures the AI starts at the first waypoint
        targVal = 0;

        //Ensures the AI begins by progressing forward through the waypoints
        forward = true;

        //Sets the current target for the AI to approach
        if (targets.Length != 0)
        {
            //Ensures the first target for the AI is set to the first waypoint in the array
            currentTarg = targets[targVal].position;
        }

        //If there are no targets that exist
        else
        {
            //The AI is set to idle in the position it is placed
            Idle();
        }

        //Sets Timer Patrol
        timerPatrol = 0f;
        //Sets Rock wait timer
        m_rockWaitTimer = 1f;

        //Checks how many waypoints are in the targets array
        if (targets.Length == 1)
        {
            //If there is only one waypoint, this is set to true to ensure the AI does not make any
            //uneccessary animations
            oneWaypoint = true;
        }

        //If there are multiple waypoints in the array
        else
        {
            //This is set to false to ensure the appropriate animations play
            oneWaypoint = false;
        }

        //Gets a bool off the player to determine whether the quick time event is currently active
        QTE = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement_Adrian>().m_bQuicktime;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks whether the AI is dead as a result of a successful quick time event by the player
        if (tag != "Dead")
        {
            //Updates the quick time event every frame to ensure as soon as the quicktime event begins, this AI does not continue
            QTE = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement_Adrian>().m_bQuicktime;

            //Calculates how far the AI is from its current target
            distanceToWaypoint = Vector3.Distance(currentTarg, agent.transform.position);

            //Checks if the AI has detected a rock throw
            if (rock)
            {
                //Ensures the AI is able to move to its new waypoint
                agent.isStopped = false;
                //Ensures the AI's animation is set to walk
                Walking();

                //Sets the new destination to the currently stored destination
                agent.SetDestination(currentTarg);

                //Checks how far away the AI is from the rocks position
                if (Vector3.Distance(agent.transform.position, currentTarg) <= 3)
                {
                    //Begins a timer to keep the AI at the rocks location for a given amount of time
                    m_rockWaitTimer += Time.deltaTime;
                    //Stops the agent from moving
                    agent.isStopped = true;
                    //Sets the animation of the agent so they look around
                    Idle();

                }

                //If the wait timer becomes greater than the given wait time
                if (m_rockWaitTimer >= m_rockWaitTime)
                {
                    //The agent is able to move
                    agent.isStopped = false;
                    //The animation is set back to walking
                    Walking();
                    //The wait timer is set back to 0 so that if it is hit again, it will begin from the start
                    m_rockWaitTimer = 0;
                    //The agent is set back to the patrol state
                    SetPatrol();
                }
            }

            //Checks if the AI is patrolling, if it is, it will set its behavior to patrol between waypoints
            if (patrolling)
            {
                Patrol();
            }

            //Checks if the AI is alerted, if it is, it will set its behavior to stop and check if the player stays within the vision cone for a certain amount of time
            if (alerted)
            {
                Alert();
            }

            //Checks if the AI is chasing, if it is, it will set its behavior to chase the player until they escape the vision cone
            if (chasing)
            {
                Chase();
            }
        }
    }

    //====================================================================
    //Uncomment for AI target debugging
    //====================================================================
    //private void OnDrawGizmos()
    //{
    //    //Draws an arrow for debug purposes to the current target of the NPC
    //    //if(agent)
    //        //Starts at the current position of the NPC, and looks in the direction of the current target
    //        //DebugExtension.DrawArrow(agent.transform.position, currentTarg - agent.transform.position, Color.magenta);
    //}

    //Behaviour for the AI to walk between waypoints assuming there are no interuptions from the player, or any other external factors
    void Patrol()
    {
        //Sets the animation of the agent to walking
        Walking();

        //Sets the agents speed to the speed passed in through the inspector
        agent.speed = m_Speed;
        //Ensures the agent can move, to avoid any conflicts when moving from other behaviours
        if (agent.isStopped && !QTE)
        {
            //Frees the agent to ensure it can move
            agent.isStopped = false;
        }

        //Checks how far the agent is from its current waypoint
        if (distanceToWaypoint >= stoppingDistance && patrolling)
        {
            //Checks that the current selected target actually exists
            if (targets.Length != 0)
            {
                //If it does exist, the position of the target becomes the new target of the agent
                agent.SetDestination(currentTarg);
            }
        }

        //Once the agent reached the waypoint
        else
        {
            //Stops Agent From Moving
            agent.isStopped = true;

            //Sets the agents animation to looking around
            if (!chasing)
            {
                Idle();
            }

            //Checks if there is more than one waypoint
            if (!oneWaypoint)
            {
                //Timer begins to increase to store how long the agent has spent at the location
                timerPatrol += Time.deltaTime;

                //Once the agent has spent the desired amount of time there
                if (timerPatrol >= waypointWaitTime)
                {
                    //Starts Agent Moving Again
                    if (agent.isStopped && !QTE)
                    {
                        agent.isStopped = false;
                    }

                    //Ensures the agent animation is set back to walking
                    Walking();

                    if (!loop)
                    {
                        //Checks if the AI has reached the end of the array of waypoints
                        if (targVal == targets.Length - 1)
                        {
                            //Sets forward to false so the AI progresses backwards through the array of waypoints
                            forward = false;
                        }

                        //If the AI was progressing backwards, this checks whether the AI has reached the end of the list
                        if (targVal == 0)
                        {
                            //And sets it to go back through the array progressing forwards
                            forward = true;
                        }

                        //If the AI is progressing forward, the next waypoint is the one after the current
                        if (forward)
                        {
                            targVal++;
                        }

                        //If the AI is progressing forward, the next waypoint is the one before the current
                        else
                        {
                            targVal--;
                        }
                    }

                    else
                    {
                        //Checks if the AI has reached the end of the array of waypoints
                        if (targVal == targets.Length - 1)
                        {
                            targVal = 0;
                        }

                        //Sets the new waypoint to the one following the current one
                        else
                        {
                            targVal++;
                        }
                    }

                    //Sets the current target to the newly determined waypoint
                    if (targets.Length != 0)
                    {
                        currentTarg = targets[targVal].position;
                    }

                    //Resets the timer back to 0 for the next waypoint delay
                    timerPatrol = 0;
                }
            }
        }
    }

    //Behaviour for the AI to stop walking if they become aware of the player, or alerted to a certain situation.
    //Stops and either moves to a chase state if it remains alerted for a certain amount of time, or goes back
    //to patrol if it does not stay alerted
    void Alert()
    {
        //Stops the agent to look "alert"
        agent.isStopped = true;

        //Sets the agents animation to looking around
        Idle();

        //Begins a timer to count how long the agent has been alert for
        timerAlert += Time.deltaTime;

        //Checks if the timer has reached a certain amount of time
        if (timerAlert >= m_fChaseWaitTime)
        {
            if (agent.isStopped && !QTE)
            {
                agent.isStopped = false;
            }
            //Speeds up the agent to run after the player at a higher speedS
            agent.speed = m_Speed * chaseSpeedMultiplier;
            //If it has, the NPC will be switched to a chase state to chase the player
            SetChase();
        }
    }

    //Behaviour for the AI to chase the player until the player escapes the AI's line of sight
    void Chase()
    {
        //Sets the agents animation to running
        Running();

        //Finds the object tagger with player
        player = GameObject.FindGameObjectWithTag("Player");

        //Sets the destination of the players position to the target for the NPC
        agent.SetDestination(player.transform.position);
        //Sets the current target to the players position to ensure the arrow is drawn towards them
        currentTarg = player.transform.position;

        timerAlert = 0;
    }

    //Sets the AI's behaviour to patrol between its waypoints
    public void SetPatrol()
    {
        if (Half != null)
        {
            Half.enabled = true;
            Open.enabled = false;
            Red.enabled = false;
        }
        patrolling = true;
        alerted = false;
        chasing = false;
        rock = false;
    }

    //Sets the AI's behaviour to act alerted by the presence of the player or a rock
    public void SetAlert()
    {
        if (Open != null)
        {
            Half.enabled = false;
            Open.enabled = true;
            Red.enabled = false;
        }
        patrolling = false;
        alerted = true;
        chasing = false;
        rock = false;
    }

    //Sets the AI's behaviour to chase the player until the player has escaped the AI's vision
    public void SetChase()
    {
        if (Red != null)
        {
            Half.enabled = false;
            Open.enabled = false;
            Red.enabled = true;
        }
        patrolling = false;
        alerted = false;
        chasing = true;
        rock = false;
    }

    //Sets the AI to approach the location the rock has landed
    public void RockThrowBools()
    {
        patrolling = false;
        alerted = false;
        chasing = false;
        rock = true;
    }

    //Sets the AI's animation to walking
    void Walking()
    {
        anim.SetBool("Walking", true);
        anim.SetBool("Running", false);
        anim.SetBool("Idle", false);
    }

    //Sets the AI's animation to running
    void Running()
    {
        anim.SetBool("Walking", false);
        anim.SetBool("Running", true);
        anim.SetBool("Idle", false);
    }

    //Sets the AI's animation to idle
    void Idle()
    {
        anim.SetBool("Walking", false);
        anim.SetBool("Running", false);
        anim.SetBool("Idle", true);
    }

    //Sets the destination of the AI's target to the location where the rock hit the ground
    public void ApproachRock(Transform pos)
    {
        agent.SetDestination(pos.position);
        currentTarg = pos.position;
    }

    //Setter for timer alert 
    public void SetAlertTimer(float alertTimer)
    {
        timerAlert = alertTimer;
    }

    //Resets the AI's position to its starting waypoint
    public void ResetPos()
    {
        transform.position = startWaypoint.position;
    }

    //Sets up the AI to prepare to be ragdolled, and to ensure it is not interfereing while the quick time event is happening
    public void preRagdoll()
    {
        anim.speed = 0;
        agent.isStopped = true;
        charCont.enabled = false;
        infectedCapsule.enabled = false;
    }

    //Ragdolls the AI and turns off everything on it to ensure it no longer interacts with the game
    public void initRagdoll()
    {
        anim.enabled = false;
        tag = "Dead";
        infectionCP.SetActive(false);
        GetComponentInChildren<DetectionOverlap>().m_bAlive = false;
        Half.enabled = true;
        Open.enabled = false;
        Red.enabled = false;
    }
}