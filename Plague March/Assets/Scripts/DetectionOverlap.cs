using UnityEngine;

public class DetectionOverlap : MonoBehaviour
{
    private AIMove_Joel moveScript;
    private bool alerted;

    // Use this for initialization
    void Start()
    {
        moveScript = GetComponentInParent<AIMove_Joel>();
        alerted = false;
    }

    // Update is called once per frame
    void Update(){}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaycastHit hitCheck;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Physics.Linecast(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 0.8f, other.transform.position.z), out hitCheck);
                Debug.DrawLine(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 0.8f, other.transform.position.z));
            }

            else
            {
                Physics.Linecast(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 1.19f, other.transform.position.z), out hitCheck);
                Debug.DrawLine(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 1.19f, other.transform.position.z));
            }

            if (!hitCheck.collider.CompareTag("Player"))
            {
                alerted = false;
            }

            else
            {
                if (!alerted)
                {
                    moveScript.SetAlert();
                }

                alerted = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RaycastHit hit;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Physics.Linecast(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 0.8f, other.transform.position.z), out hit);
                Debug.DrawLine(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 0.8f, other.transform.position.z));
            }

            else
            {
                Physics.Linecast(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 1.19f, other.transform.position.z), out hit);
                Debug.DrawLine(new Vector3(GetComponentInParent<Transform>().position.x, GetComponentInParent<Transform>().position.y + 1.75f, GetComponentInParent<Transform>().position.z), new Vector3(other.transform.position.x, other.transform.position.y + 1.19f, other.transform.position.z));
            }

            if(!hit.collider.CompareTag("Player"))
            {
                alerted = false;
                moveScript.SetPatrol();
                moveScript.SetAlertTimer(0);
            }

            else
            {
                if (!alerted)
                {
                    moveScript.SetAlert();
                }

                alerted = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        moveScript.SetPatrol();
        moveScript.SetAlertTimer(0);
        alerted = false;
    }
}
