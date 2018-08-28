using UnityEngine;
using UnityEngine.AI;

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

    //Used to time how long is spent at the current waypoint, once arrived
    public float waypointWaitTime;
    //Current Distance the agent is away from the Waypoint
    private float distanceToWaypoint;

    //Stores a reference to the player, making their position easily obtainable
    [HideInInspector] public GameObject player;
    //Obtains reference to the agent to set its targets etc
    private NavMeshAgent agent;


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
    private Transform currentTarg;

    //Stores the animator of the actor in which needs to be altered
    private Animator anim;

    //Used to iterate randomly through the waypoints
    private int i;

    // Use this for initialization
    void Start ()
    {
        //Gets the NavMesh agent component from the AI to set waypoints and such
        agent = GetComponent<NavMeshAgent>();
        //Gets the Animator component from the AI to alter the animations
        anim = GetComponent<Animator>();

        //Sets the AI's default behaviour to patrol
        SetPatrol();

        //Randomly sets the first waypoint for the AI to walk towards
        i = Random.Range(0, targets.Length);

        currentTarg = targets[i];

        //Sets Timer Patrol
        timerPatrol = 0f;
        //Sets Rock wait timer
        m_rockWaitTimer = 1f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Calculates how far the AI is from its current target
        distanceToWaypoint = Vector3.Distance(targets[i].position, agent.transform.position);

        if (rock)
        {
            //Ensures the AI is able to move to its new waypoint
            agent.isStopped = false;
            //Ensures the AI's animation is set to walk
            anim.SetFloat("Blend", 0.0f);

            //Sets the new destination to the currently stored destination
            agent.SetDestination(currentTarg.position);

            //Checks how far away the AI is from the rocks position
            if (Vector3.Distance(agent.transform.position, currentTarg.position) <= 1)
            {
                //Begins a timer to keep the AI at the rocks location for a given amount of time
                m_rockWaitTimer += Time.deltaTime;
                //Stops the agent from moving
                agent.isStopped = true;
                //Sets the animation of the agent so they look around
                anim.SetFloat("Blend", 0.5f);

            }

            //If the wait timer becomes greater than the given wait time
            if (m_rockWaitTimer >= m_rockWaitTime)
            {
                //The agent is able to move
                agent.isStopped = false;
                //The animation is set back to walking
                anim.SetFloat("Blend", 0.0f);
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

    private void OnDrawGizmos()
    {
        //Draws an arrow for debug purposes to the current target of the NPC
        if(agent)
            //Starts at the current position of the NPC, and looks in the direction of the current target
            DebugExtension.DrawArrow(agent.transform.position, currentTarg.position - agent.transform.position, Color.magenta);
    }

    void Patrol()
    {
        //Sets the animation of the agent to walking
        anim.SetFloat("Blend", 0.0f);

        //Sets the agents speed to the speed passed in through the inspector
        agent.speed = m_Speed;
        //Ensures the agent can move, to avoid any conflicts when moving from other behaviours
        agent.isStopped = false;

        //Checks how far the agent is from its current waypoint
        if (distanceToWaypoint >= stoppingDistance)
        {
            //Checks that the current selected target actually exists
            if (targets[i] != null)
            {
                //If it does exist, the position of the target becomes the new target of the agent
                agent.SetDestination(currentTarg.position);
            }
        }

        //Once the agent reached the waypoint
        else
        {
            //Stops Agent From Moving
            agent.isStopped = true;

            //Sets the agents animation to looking around
            anim.SetFloat("Blend", 0.5f);

            //Timer begins to increase to store how long the agent has spent at the location
            timerPatrol += Time.deltaTime;

            //Once the agent has spent the desired amount of time there
            if (timerPatrol >= waypointWaitTime)
            {
                //Starts Agent Moving Again
                agent.isStopped = false;

                //Ensures the agent animation is set back to walking
                anim.SetFloat("Blend", 0.0f);

                //Used to store the current waypoint, to ensure that the current waypoint is not set to the new waypoint
                int tempi = i;
                //Randomly assigns a new waypoint
                i = Random.Range(0, targets.Length);

                //Loops every time the new waypoint is reset to the same as the current waypoint, until a different one is selected
                if (targets.Length > 1)
                {
                    while (i == tempi)
                    {
                        //Randomly assigns a new waypoint
                        i = Random.Range(0, targets.Length);
                    }
                    currentTarg = targets[i];
                }

                //Resets the timer back to 0 for the next waypoint delay
                timerPatrol = 0;
            }
        }
    }

    void Alert()
    {
        //Stops the agent to look "alert"
        agent.isStopped = true;

        //Sets the agents animation to looking around
        anim.SetFloat("Blend", 0.5f);

        //Begins a timer to count how long the agent has been alert for
        timerAlert += Time.deltaTime;

        //Checks if the timer has reached a certain amount of time
        if(timerAlert >= 3.0f)
        {
            //If it has, the NPC will be switched to a chase state to chase the player
            SetChase();
        }
    }

    void Chase()
    {
        //Sets the agents animation to running
        anim.SetFloat("Blend", 1.0f);
        //Speeds up the agent to run after the player at a higher speedS
        agent.speed = m_Speed * chaseSpeedMultiplier;

        //Ensures the agent can move, only getting to this state from the alert state which has stopped the NPC
        agent.isStopped = false;
        
        //Finds the object tagger with player
        player = GameObject.FindGameObjectWithTag("Player");

        //Sets the destination of the players position to the target for the NPC
        agent.SetDestination(player.transform.position);
        //Sets the current target to the players position to ensure the arrow is drawn towards them
        currentTarg = player.transform;

        timerAlert = 0;
    }

    public void SetPatrolPoint(Transform pos)
    {
        //Sets the agents current destination to the passed in position
        agent.SetDestination(pos.position);
        //Stores the current target of the agent
        currentTarg = pos;

        if (Vector3.Distance(pos.position, agent.transform.position) <= 5.0f)
        {
            //Used to store the current waypoint, to ensure that the current waypoint is not set to the new waypoint
            int tempi = i;
            //Randomly assigns a new waypoint
            i = Random.Range(0, targets.Length);

            //Loops every time the new waypoint is reset to the same as the current waypoint, until a different one is selected
            while (i == tempi)
            {
                //Randomly assigns a new waypoint
                i = Random.Range(0, targets.Length);
            }

            rock = false;
            SetPatrol();
        }
    }

    public void SetPatrol()
    {
        patrolling = true;
        alerted = false;
        chasing = false;
        rock = false;
    }

    public void SetAlert()
    {
        patrolling = false;
        alerted = true;
        chasing = false;
        rock = false;
    }

    public void SetChase()
    {
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
        currentTarg = pos;
    }

    public void ApproachLastPos(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
}


