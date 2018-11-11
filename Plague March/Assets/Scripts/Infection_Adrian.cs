using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class Infection_Adrian : MonoBehaviour {

    //Infection Level
    [Range(0f, 100.0f)] [SerializeField] public float m_fInfection = 0f;

    //Distance till infection starts
    [Range(0f, 100.0f)] [SerializeField] float m_fDistanceUntilInfection = 0f;
    
    //Infection Multiplyer For Infection Level
    public float m_fInfectionMultiplyer = 0;

    //How Quick Infection Is reduced
    public float m_freduceInfection = 0;

    //How close the ai is before infection takes effect
    //float m_fInfectionDistance = 0;

    //List of enemies
    public List<Transform> m_lEnemies;

    //Distance to closest enemy
    float m_fDistanceToNearestEnemy;

    public Image YouDead = null;
    public Image InfectionBar = null;

    private Movement_Adrian moveScript;

    public NavMeshAgent[] agentGroup;

    private void Awake()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Infected"))
        {
            m_lEnemies.Add(go.GetComponent<Transform>());
        }

        GameObject scriptGetter = GameObject.FindGameObjectWithTag("Player");
        moveScript = scriptGetter.GetComponent<Movement_Adrian>();
    }

	// Update is called once per frame
	void Update ()
    {
        //Finds distance to closest enemy
        if (m_lEnemies != null)
        {
            m_fDistanceToNearestEnemy = Vector3.Distance(GetClosestEnemy(m_lEnemies, this.transform).position, this.transform.position);

            //Increasing infection amount
            Material mat = Instantiate(InfectionBar.material);
            mat.SetFloat("_opacity", m_fInfection / 100);
            InfectionBar.material = mat;

            //If Distance is close
            if (m_fDistanceToNearestEnemy <= m_fDistanceUntilInfection)
            {
                //Accumulate Infection over time using the multiplyer
                m_fInfection += m_fInfectionMultiplyer * Time.deltaTime;
            }
            else
            {
                //Reduce Infection Over time
                m_fInfection -= m_freduceInfection * Time.deltaTime;

                if (m_fInfection <= 0)
                {
                    m_fInfection = 0;
                }
            }

            if (m_fInfection >= 100)
            {
                
            }

            Debug.Log(moveScript.m_bQuicktime);
            //=========================================================================================================
            if(moveScript.m_bQuicktime)
            {
                if(agentGroup != null)
                {
                    for (int i = 0; i < agentGroup.Length; i++)
                    {
                        agentGroup[i].isStopped = true;
                    }
                }
            }
            //======================================================================================================
        }
	}

    //Where I found this code
    //https://answers.unity.com/questions/1236558/finding-nearest-game-object.html
    public Transform GetClosestEnemy(List<Transform> ListOfEnemies, Transform fromThis)
    {
        //Best Target
        Transform bestTarget = null;

        //ClosestDistanceSquare
        float closestDistanceSqr = Mathf.Infinity;

        //Current position of player
        Vector3 currentPosition = fromThis.position;

        //For each enemy in the list of enemies
        foreach (Transform Enemy in ListOfEnemies)
        {

            Vector3 directionToTarget = Enemy.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = Enemy;
            }
        }
        //Returns best target according to distance
        return bestTarget;
    }
}
