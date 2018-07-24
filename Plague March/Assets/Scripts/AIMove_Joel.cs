using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove_Joel : MonoBehaviour
{
    public Transform[] targets;
    public float WaypointWaitTime;

    [HideInInspector]
    public GameObject player; //DO NOT SET, JUST FOR VIEWING IN INSPECTOR

    public MeshCollider cone;

    private CharacterController control;
    private NavMeshAgent agent;

    private int i;
    private float timer = 0;

    private bool patrolling;
    private bool alerted;
    private bool chasing;

    // Use this for initialization
    void Start ()
    {
        control = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        SetPatrol();
        i = Random.Range(0, targets.Length - 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (patrolling)
            Patrol();

        else if (alerted)
            Alert();

        else if (chasing)
            Chase();
    }

    void Patrol()
    {
        agent.isStopped = false;

        if (Vector3.Distance(targets[i].position, agent.transform.position) >= 2.0f)
        {
            if (targets[i] != null)
            {
                agent.SetDestination(targets[i].position);
            }
        }

        else
        {
            timer += Time.deltaTime;

            if (timer >= WaypointWaitTime)
            {
                int tempi = i;
                i = Random.Range(0, targets.Length - 1);

                while(i == tempi)
                {
                    i = Random.Range(0, targets.Length - 1);
                }

                timer = 0;
            }
        }
    }

    void Alert()
    {
        agent.isStopped = true;

        timer += Time.deltaTime;

        if(timer >= 3.0f)
        {
            SetChase();
        }
    }

    void Chase()
    {
        Debug.Log("CHASE");

        agent.isStopped = false;
        
        player = GameObject.FindGameObjectWithTag("Player");

        agent.SetDestination(player.transform.position);
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
}
