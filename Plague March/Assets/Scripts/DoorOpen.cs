using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorOpen : MonoBehaviour
{
    public enum ROOM {exterior, roomInterior, medicInterior, chruchInterior};
    public ROOM arrivalRoom;

    public Text tooltipText;

    private bool inTrigger;

    // Use this for initialization
    void Start()
    {
        tooltipText.enabled = false;
        inTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && inTrigger)
        {
            SceneManager.LoadScene((int)arrivalRoom);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HIT");
            tooltipText.enabled = true;
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HIT");
            tooltipText.enabled = false;
            inTrigger = false;
        }
    }
}
