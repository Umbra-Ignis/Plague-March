using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class QuickTimeEvent_Adrian : MonoBehaviour
{

    //Players movement script
    Movement_Adrian m_maMoveScript;

    //Infection Script
    Infection_Adrian m_iaInfectionScript;

    //List of enemies
    public List<Transform> m_lEnemies;

    //Start quicktime event distance
    [Range(0f, 10.0f)] [SerializeField] float m_fStartQuickTimeDistance = 0f;

    //How Many times to press quicktime button
    [Range(0f, 10.0f)] [SerializeField] float m_fHowManyPresses = 0f;

    //Start quicktime event distance
    [Range(0f, 10.0f)] [SerializeField] float m_fTimeRemoved = 0f;

    //Distance to closest enemy
    float m_fDistanceToClosestEnemy;

    //Is in event
    bool m_bInEvent = false;

    //Times Pressed
    float m_fTimesPressed = 0;

    //Closest enemy
    Transform Closestenemy;

    //Joels Ai Script
    AIMove_Joel Ai;

    //Timer Until Attack Again
    float m_fAttackAgain;

    public Image Bar = null;
    public Image Border = null;





    // Use this for initialization
    void Start()
    {
        m_iaInfectionScript = GetComponent<Infection_Adrian>();
        m_maMoveScript = GetComponent<Movement_Adrian>();

    }

    private void Awake()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Infected"))
        {
            m_lEnemies.Add(go.GetComponent<Transform>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_lEnemies != null)
        {

            //gets closest enemy
            m_fDistanceToClosestEnemy = Vector3.Distance(m_iaInfectionScript.GetClosestEnemy(m_lEnemies, transform).position, transform.position);


            if (m_fDistanceToClosestEnemy >= m_fStartQuickTimeDistance)
            {
                m_fAttackAgain = 0;
                m_fTimesPressed = 0;
            }

            //if distance to the closest enemy is less than or equal to distance set in inspector
            if (m_fDistanceToClosestEnemy <= m_fStartQuickTimeDistance)
            {
                Border.enabled = true;
                Bar.enabled = true;
                Bar.fillAmount = m_fTimesPressed / 100;

                Closestenemy = m_iaInfectionScript.GetClosestEnemy(m_lEnemies, transform);

                Ai = Closestenemy.GetComponentInParent<AIMove_Joel>();

                if (m_fAttackAgain <= 0)
                {
                    Ai.SetChase();

                    // FIX THIS
                    //transform.LookAt(Closestenemy);
                    //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);

                    Debug.Log("QUICK");
                    //Hold players Position
                    m_maMoveScript.SetQuicktime(true);
                }
                Ai.agent.isStopped = true;
                Ai.anim.SetFloat("Blend", 0.0f);


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_fTimesPressed += m_fHowManyPresses;
                    Debug.Log("Pressed");
                }

                //Increase over time
                m_fTimesPressed -= m_fTimeRemoved * Time.deltaTime;

                //Reset Under 0
                if (m_fTimesPressed < 0)
                {
                    m_fTimesPressed = 0;
                }

                m_fAttackAgain += 1 * Time.deltaTime;

                if (m_fTimesPressed >= 100)
                {
                    Border.enabled = false;
                    Bar.enabled = false;
                    m_maMoveScript.SetQuicktime(false);
                    Ai.SetPatrol();
                }
            }
        }
    }
}
