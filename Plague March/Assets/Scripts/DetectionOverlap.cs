//========================================================================================
//Detection Overlap
//
//Functionality: Used to check if the AI can draw line of sight to the player, and in turn
//used to switch between behaviours 
//
//Author: Joel G
//Altered by: Adrian P
//========================================================================================
using UnityEngine;

public class DetectionOverlap : MonoBehaviour
{
    //Stores a reference to the AI movement script associated with THIS AI
    private AIMove_Joel m_csMoveScript;
    //Stores whether the player has been detected 
    private bool m_bAlerted;

    //Stores whether THIS AI is still alive, or has been removed from the game
    //as part of a quicktime event
    [HideInInspector]
    public bool m_bAlive;

    // Use this for initialization
    void Start()
    {
        //Gets reference to the movement script on THIS AI
        m_csMoveScript = GetComponentInParent<AIMove_Joel>();
        //Sets alerted to false initially as no AI commence the game alerted
        m_bAlerted = false;
        //Sets alive to true initially as no AI commence the game dead
        m_bAlive = true;
    }

    void Update(){} //DELIBERATELY LEFT BLANK

    //Called whenever another collider enters THIS collider
    private void OnTriggerEnter(Collider other)
    {
        //Checks if the other collider is one attached to the player, and if THIS AI is alive
        if (other.CompareTag("Player") && m_bAlive)
        {
            //Stores the raycast that is calculated below
            RaycastHit m_rHitCheck;

            //Checks whether the player is crouched
            if (Input.GetKey(KeyCode.LeftControl))
            {
                //If the player is crouched the raycast is set lower to take this into account
                Physics.Linecast(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 0.8f, other.transform.position.z), out m_rHitCheck);
                Debug.DrawLine(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 0.8f, other.transform.position.z));
            }

            //If the player is not crouched
            else
            {
                //The raycast is performed at a standard height
                Physics.Linecast(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 1.19f, other.transform.position.z), out m_rHitCheck);
                Debug.DrawLine(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 1.19f, other.transform.position.z));
            }

            //Checks if the tag of the other collider is NOT player
            if (!m_rHitCheck.collider.CompareTag("Player"))
            {
                //Ensures the state is not switched to alerted
                m_bAlerted = false;
            }

            //If the tag of the other collider IS player
            else
            {
                //And if the AI isn't already alerted
                if (!m_bAlerted)
                {
                    //The AI is then set to alerted behaviour
                    m_csMoveScript.SetAlert();
                }

                //Ensures the local alerted variable is set to true
                m_bAlerted = true;
            }
        }
    }

    //Called while another collider is within THIS one
    private void OnTriggerStay(Collider other)
    {
        //Checks if the other collider is one attached to the player, and if THIS AI is alive
        if (other.CompareTag("Player") && m_bAlive)
        {
            //Stores the raycast that is calculated below
            RaycastHit m_rHit;

            //Checks whether the player is crouched
            if (Input.GetKey(KeyCode.LeftControl))
            {
                //If the player is crouched the raycast is set lower to take this into account
                Physics.Linecast(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 0.8f, other.transform.position.z), out m_rHit);
                Debug.DrawLine(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 0.8f, other.transform.position.z));
            }

            //If the player is not crouched
            else
            {
                //The raycast is performed at a standard height
                Physics.Linecast(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 1.19f, other.transform.position.z), out m_rHit);
                Debug.DrawLine(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 1.19f, other.transform.position.z));
            }

            //Checks if the tag of the other collider is NOT player
            if (!m_rHit.collider.CompareTag("Player"))
            {
                m_bAlerted = false;
                m_csMoveScript.SetPatrol();
                m_csMoveScript.SetAlertTimer(0);
            }

            //If the tag of the other collider IS player
            else
            {
                //And if the AI isn't already alerted
                if (!m_bAlerted)
                {
                    //The AI is then set to alerted behaviour
                    m_csMoveScript.SetAlert();
                }

                //Ensures the local alerted variable is set to true
                m_bAlerted = true;
            }
        }
    }

    //Called when a collider exits THIS collider
    private void OnTriggerExit(Collider other)
    {
        //Checks if the other collider is one attached to the player, and if THIS AI is alive
        if (other.CompareTag("Player") && m_bAlive)
        {
            //Sets THIS AI back to a patrol behaviour between waypoints
            m_csMoveScript.SetPatrol();
            //Ensures the alert timer is reset to avoid any weird behaviour
            m_csMoveScript.SetAlertTimer(0);
            //Ensures the AI is set back to unalerted
            m_bAlerted = false;
        }
    }
}
