using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Infection_Adrian : MonoBehaviour {

    //Infection Level
    [Range(0f, 100.0f)] [SerializeField] float m_fInfection = 0f;

    //Distance till infection starts
    [Range(0f, 100.0f)] [SerializeField] float m_fDistanceUntilInfection = 0f;
    
    //Infection Multiplyer For Infection Level
    public float m_fInfectionMultiplyer = 0;

    //How close the ai is before infection takes effect
    //float m_fInfectionDistance = 0;

    //List of enemies
    public List<Transform> m_lEnemies;

    //Distance to closest enemy
    float m_fDistanceToNearestEnemy;

    public Image YouDead = null;
    public Image InfectionBar = null;

    



    private void Awake()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Infected"))
        {
            m_lEnemies.Add(go.GetComponent<Transform>());
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Finds distance to closest enemy
        m_fDistanceToNearestEnemy = Vector3.Distance(GetClosestEnemy(m_lEnemies, this.transform).position, this.transform.position);

        //If Distance is close
        if (m_fDistanceToNearestEnemy <= m_fDistanceUntilInfection)
        {
            //Accumulate Infection over time using the multiplyer
            m_fInfection += m_fInfectionMultiplyer * Time.deltaTime;
            InfectionBar.fillAmount = m_fInfection / 100;
        }

        if (m_fInfection >= 100)
        {
            YouDead.enabled = true;
            Time.timeScale = 0f;
            //END GAME
        }
	}

    //Where i found this code
    //https://answers.unity.com/questions/1236558/finding-nearest-game-object.html
    Transform GetClosestEnemy(List<Transform> ListOfEnemies, Transform fromThis)
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
