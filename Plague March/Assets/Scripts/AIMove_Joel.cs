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
    [Range(1f, 50f)] public float stoppingDistance = 4f;
    //Sets how much faster the chasing state will be, 1 being the same speed as patrol
    [Range(1f, 20f)]public float chaseSpeedMultiplier;
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

    public Image Open = null;
    public Image Half = null;
    public Image Red = null;

    private bool oneWaypoint;

    public Transform startWaypoint;
    private CharacterController charCont;
    private CapsuleCollider infectedCapsule;

    private bool QTE;

    public GameObject infectionCP;

    // Use this for initialization
    void Start ()
    {
        //Gets the NavMesh agent component from the AI to set waypoints and such
        agent = GetComponent<NavMeshAgent>();
        //Gets the Animator component from the AI to alter the animations
        anim = GetComponent<Animator>();

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
            currentTarg = targets[targVal].position;
        }
        else
        {
            Idle();
        }

        //Sets Timer Patrol
        timerPatrol = 0f;
        //Sets Rock wait timer
        m_rockWaitTimer = 1f;

        if(targets.Length == 1)
        {
            oneWaypoint = true;
        }

        else
        {
            oneWaypoint = false;
        }

        QTE = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement_Adrian>().m_bQuicktime;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (tag != "Dead")
        {
            QTE = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement_Adrian>().m_bQuicktime;

            //Calculates how far the AI is from its current target
            distanceToWaypoint = Vector3.Distance(currentTarg, agent.transform.position);

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
            agent.isStopped = false;
        }

        //Checks how far the agent is from its current waypoint
        if (distanceToWaypoint >= stoppingDistance)
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

            if(!oneWaypoint)
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
                        if (targVal == targets.Length - 1)
                        {
                            targVal = 0;
                        }

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
        Running();

        //Begins a timer to count how long the agent has been alert for
        timerAlert += Time.deltaTime;

        //Checks if the timer has reached a certain amount of time
        if(timerAlert >= m_fChaseWaitTime)
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

    //Sets the new patrol point for the AI, generally used from other scripts to allow for situational patrol points to be added
    public void SetPatrolPoint(Transform pos)
    {
        //Sets the agents current destination to the passed in position
        agent.SetDestination(pos.position);
        //Stores the current target of the agent
        currentTarg = pos.position;

        if (Vector3.Distance(pos.position, agent.transform.position) <= 5.0f)
        {
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
                if (targVal == targets.Length - 1)
                {
                    targVal = 0;
                }

                else
                {
                    targVal++;
                }
            }

            //Sets the current target to the newly determined waypoint
            //currentTarg = targets[targVal].position;
            rock = false;
            SetPatrol();
        }
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
    public void RockThrowBools()
    {
        patrolling = false;
        alerted = false;
        chasing = false;
        rock = true;
    }

    public void ApproachRock(Transform pos)
    {
        agent.SetDestination(pos.position);
        currentTarg = pos.position;
    }

    public void ApproachLastPos(Vector3 LastPosititon)
    {
        //Last position player exit detection cone
        currentTarg = LastPosititon;
    }
    
    //Setter for timer alert 
    public void SetAlertTimer(float alertTimer)
    {
        timerAlert = alertTimer;
    }

    public void ResetPos()
    {
        transform.position = startWaypoint.position;
    }

    public void preRagdoll()
    {
        anim.speed = 0;
        agent.isStopped = true;
        charCont.enabled = false;
        infectedCapsule.enabled = false;
    }

    public void initRagdoll()
    {
        anim.enabled = false;
        tag = "Dead";
        infectionCP.SetActive(false);
        GetComponentInChildren<DetectionOverlap>().alive = false;

    }

    void Walking()
    {
        anim.SetBool("Walking", true);
        anim.SetBool("Running", false);
        anim.SetBool("Idle", false);
    }

    void Running()
    {
        anim.SetBool("Walking", false);
        anim.SetBool("Running", true);
        anim.SetBool("Idle", false);
    }

    void Idle()
    {
        anim.SetBool("Walking", false);
        anim.SetBool("Running", false);
        anim.SetBool("Idle", true);
    }
}