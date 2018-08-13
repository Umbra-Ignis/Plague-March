using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.Animations;

public class AIMove_Joel : MonoBehaviour
{
    //Takes in an array of waypoints for the AI to patrol between
    public Transform[] targets;

    //Determines Speed
    [Range(1f, 50f)] public float m_Speed = 1f;

    //Distance agent Will stop Relative to waypoint
    [Range(1f, 50f)] public float DistanceFromWaypoint = 4f;

    //Used to time how long is spent at the current waypoint, once arrived
    public float WaypointWaitTime;

    //Current Distance From Waypoint
    public float DistanceToWaypoint;

    //Stores a reference to the player, making their position easily obtainable
    [HideInInspector]
    public GameObject player;

    //Obtains reference to the agent to set its targets etc
    private NavMeshAgent agent;

    //Used to iterate randomly through the waypoints
    private int i;

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
    public Transform currentTarg;

    //Stores the animator of the actor in which needs to be altered
    private Animator anim;

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
        i = Random.Range(0, targets.Length - 1);

        //Sets the default animation to walking
        anim.SetFloat("Blend", 0.0f);

        //Sets Timer Patrol
        timerPatrol = 0f;
        //Sets Rock wait timer
        m_rockWaitTimer = 0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Calculates how far the AI is from its current target
        DistanceToWaypoint = Vector3.Distance(targets[i].position, agent.transform.position);

        //DEBUG FOR ROCK THROW
        //=====================================================================
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    RockThrowBools();
        //}

        if (rock)
        {
            agent.isStopped = false;
            anim.SetFloat("Blend", 0.0f);

            agent.SetDestination(currentTarg.position);
            if (Vector3.Distance(agent.transform.position, currentTarg.position) <= 1)
            {
                m_rockWaitTimer += Time.deltaTime;
                agent.isStopped = true;
                anim.SetFloat("Blend", 0.5f);

            }
            if (m_rockWaitTimer >= m_rockWaitTime)
            {
                agent.isStopped = false;
                anim.SetFloat("Blend", 0.0f);
                m_rockWaitTimer = 0;
                SetPatrol();
            }
        }
        //=====================================================================

        //Checks if the AI is patrolling, if it is, it will set its behavior to patrol between waypoints
        if (patrolling)
        {
            Debug.Log("PATROL BOOL");
            Patrol();
        }

        //Checks if the AI is alerted, if it is, it will set its behavior to stop and check if the player stays within the vision cone for a certain amount of time
        if (alerted)
        {
            Debug.Log("ALERT BOOL");
            Alert();
        }

        //Checks if the AI is chasing, if it is, it will set its behavior to chase the player until they escape the vision cone
        if (chasing)
        {
            Debug.Log("CHASING BOOL");
            Chase();
        }

        //if (rock)
        //{
        //    Debug.Log("ROCK BOOL");
        //    ApproachRock(currentTarg);
        //}
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
        anim.SetFloat("Blend", 0.0f);

        agent.speed = m_Speed;
        //Ensures the agent can move, to avoid any conflicts when moving from other behaviours
        agent.isStopped = false;

        //Checks how far the agent is from its current waypoint
        if (DistanceToWaypoint >= DistanceFromWaypoint)
        {
            Debug.Log("Stopped");
            //Checks that the current selected target actually exists
            if (targets[i] != null)
            {
                //If it does exist, the position of the target becomes the new target of the agent
                agent.SetDestination(targets[i].position);
                currentTarg = targets[i];
            }
        }
        else //Once the agent reached the waypoint
        {
            //Stops Agent From Moving
            agent.isStopped = true;

            anim.SetFloat("Blend", 0.5f);

            //Timer begins to increase to store how long the agent has spent at the location
            timerPatrol += Time.deltaTime;

            //Once the agent has spent the desired amount of time there
            if (timerPatrol >= WaypointWaitTime)
            {
                //Starts Agent Moving Again
                agent.isStopped = false;

                anim.SetFloat("Blend", 0.0f);

                //Used to store the current waypoint, to ensure that the current waypoint is not set to the new waypoint
                int tempi = i;
                //Randomly assigns a new waypoint
                i = Random.Range(0, targets.Length - 1);

                //Loops every time the new waypoint is reset to the same as the current waypoint, until a different one is selected
                if (targets.Length > 1)
                {
                    while (i == tempi)
                    {
                        //Randomly assigns a new waypoint
                        i = Random.Range(0, targets.Length - 1);
                    }
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

    void ReturnToPatrol()
    {

    }

    void Chase()
    {
        Debug.Log("CHASE");

        anim.SetFloat("Blend", 1.0f);
        agent.speed = 2.0f;

        //Ensures the agent can move, only getting to this state from the alert state which has stopped the NPC
        agent.isStopped = false;
        
        //Finds the object tagger with player
        player = GameObject.FindGameObjectWithTag("Player");

        //Sets the destination of the players position to the target for the NPC
        agent.SetDestination(player.transform.position);
        //Sets the current target to the players position to ensure the arrow is drawn towards them
        currentTarg = player.transform;
    }

    public void SetPatrolPoint(Transform pos)
    {
        agent.SetDestination(pos.position);
        currentTarg = pos;

        if (Vector3.Distance(pos.position, agent.transform.position) <= 5.0f)
        {
            //Used to store the current waypoint, to ensure that the current waypoint is not set to the new waypoint
            int tempi = i;
            //Randomly assigns a new waypoint
            i = Random.Range(0, targets.Length - 1);

            //Loops every time the new waypoint is reset to the same as the current waypoint, until a different one is selected
            while (i == tempi)
            {
                //Randomly assigns a new waypoint
                i = Random.Range(0, targets.Length - 1);
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

    public bool GetPatrolBool()
    {
        return patrolling;
    }

    public bool GetAlertlBool()
    {
        return alerted;
    }

    public bool GetChaseBool()
    {
        return chasing;
    }


    public void ApproachRock(Transform pos)
    {
        agent.SetDestination(pos.position);
        currentTarg = pos;
    }
    //private void OnTriggerStay(Collider other)
    //{


    //    if (other.gameObject.CompareTag("Rock"))
    //    {
            
    //        //collision.collider.tag.Replace("Rock", "Untagged");
    //        if (TestTimer >= m_rockWaitTimer)
    //            SetPatrol();
    //        else
    //        {
    //            if (TestTimer <= m_rockWaitTimer)
    //            {
    //                agent.isStopped = true;
    //                anim.SetFloat("Blend", 0.5f);
    //            }
    //            else
    //            {
    //                agent.isStopped = false;
    //                anim.SetFloat("Blend", 0.0f);
    //                TestTimer = 0;
    //                SetPatrol();
    //            }
    //        }
    //    }
    //}

    

}


