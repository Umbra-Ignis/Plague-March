using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.Animations;

public class AIMove_Joel : MonoBehaviour
{
    //Takes in an array of waypoints for the AI to patrol between
    public Transform[] targets;
    //Determines the amount of time the AI spends at each of the waypoints
    public float WaypointWaitTime;

    [HideInInspector]
    public GameObject player; //DO NOT SET, JUST FOR VIEWING IN INSPECTOR

    //Obtains reference to the agent to set its targets etc
    private NavMeshAgent agent;

    //Used to iterate randomly through the waypoints
    private int i;
    //Used to time how long is spent at the current waypoint, once arrived
    private float timer = 0;

    //Stores whether the AI is patrolling
    private bool patrolling;
    //Stores whether the AI is alerted
    private bool alerted;
    //Stores whether the AI is chasing
    private bool chasing;
    private bool rock;

    public Transform TESTPOS;

    private Transform currentTarg;

    private Animator anim;
    private BlendTree bt;

    // Use this for initialization
    void Start ()
    {
        //Gets the NavMesh agent component from the AI to set waypoints and such
        agent = GetComponent<NavMeshAgent>();
        //Sets the AI's default behaviour to patrol
        SetPatrol();
        //Randomly sets the first waypoint for the AI to walk towards
        i = Random.Range(0, targets.Length - 1);
        rock = false;

        anim = GetComponent<Animator>();
        bt = anim.GetComponent<BlendTree>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //DEBUG FOR ROCK THROW
        //=====================================================================
        if(Input.GetKeyDown(KeyCode.L))
        {
            rock = true;
            patrolling = false;
        }

        if(rock)
            SetPatrolPoint(TESTPOS);
        //=====================================================================

        //Checks if the AI is patrolling, if it is, it will set its behavior to patrol between waypoints
        if (patrolling)
            Patrol();

        //Checks if the AI is alerted, if it is, it will set its behavior to stop and check if the player stays within the vision cone for a certain amount of time
        else if (alerted)
            Alert();

        //Checks if the AI is chasing, if it is, it will set its behavior to chase the player until they escape the vision cone
        else if (chasing)
            Chase();
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
        agent.speed = 1.0f;

        //Ensures the agent can move, to avoid any conflicts when moving from other behaviours
        agent.isStopped = false;

        //Checks how far the agent is from its current waypoint
        if (Vector3.Distance(targets[i].position, agent.transform.position) >= 2.0f)
        {
            //Checks that the current selected target actually exists
            if (targets[i] != null)
            {
                //If it does exist, the position of the target becomes the new target of the agent
                agent.SetDestination(targets[i].position);
                currentTarg = targets[i];
            }
        }

        //Once the agent reached the waypoint
        else
        {
            //Timer begins to increase to store how long the agent has spent at the location
            timer += Time.deltaTime;

            //Once the agent has spent the desired amount of time there
            if (timer >= WaypointWaitTime)
            {
                //Used to store the current waypoint, to ensure that the current waypoint is not set to the new waypoint
                int tempi = i;
                //Randomly assigns a new waypoint
                i = Random.Range(0, targets.Length - 1);

                //Loops every time the new waypoint is reset to the same as the current waypoint, until a different one is selected
                while(i == tempi)
                {
                    //Randomly assigns a new waypoint
                    i = Random.Range(0, targets.Length - 1);
                }

                //Resets the timer back to 0 for the next waypoint delay
                timer = 0;
            }
        }
    }

    void Alert()
    {
        //Stops the agent to look "alert"
        agent.isStopped = true;

        anim.SetFloat("Blend", 0.5f);

        //Begins a timer to count how long the agent has been alert for
        timer += Time.deltaTime;

        //Checks if the timer has reached a certain amount of time
        if(timer >= 3.0f)
        {
            //If it has, the NPC will be switched to a chase state to chase the player
            SetChase();
        }
    }

    void Chase()
    {
        Debug.Log("CHASE");

        timer = 0;
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

    public void SetPatrol()
    {
        patrolling = true;
        alerted = false;
        chasing = false;
    }

    public void SetAlert()
    {
        patrolling = false;
        alerted = true;
        chasing = false;
    }

    public void SetChase()
    {
        patrolling = false;
        alerted = false;
        chasing = true;
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

    public void SetPatrolPoint(Transform pos)
    {
        agent.SetDestination(pos.position);
        currentTarg = pos;

        if (Vector3.Distance(pos.position, agent.transform.position) <= 10.0f)
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
}
