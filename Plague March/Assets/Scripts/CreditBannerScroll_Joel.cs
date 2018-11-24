//========================================================================================
//CreditBannerScroll_Joel
//
//Functionality: Used to move the credits up the screen whilst in the credits scene
//
//Author: Joel G
//========================================================================================
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditBannerScroll_Joel : MonoBehaviour
{
    //Stores a reference to the image used for the credits
    private Image bannerImg;
    //Takes in a float as to how much to increase the scrolling speed of the credits
    public float scrollSpeedMultiplier;
    //Takes a reference to an image to pop up once the credits have finished
    public Image tooltipImage;

    // Use this for initialization
    void Start()
    {
        //Obtains the reference to the credits image
        bannerImg = GetComponent<Image>();
        //Ensures the tooltip image is turned off
        tooltipImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the banner is below the top of the screen
        if (bannerImg.transform.position.y <= 2000)
        {
            //If it is, it progresses it up the screen to give the scrolling effect
            bannerImg.transform.Translate(new Vector3(0.0f, 1.0f * scrollSpeedMultiplier, 0.0f));
        }
        
        //Checks if the banner is above the top of the screen
        if(bannerImg.transform.position.y >= 2000)
        {
            //If it is, it will turn on the tooltip image, indicating to the player how to move on
            tooltipImage.enabled = true;
        }

        //Checks if the credits have finished, and if the player chooses to continue by pressing space
        if(tooltipImage.enabled == true && Input.GetKeyDown(KeyCode.Space))
        {
            //Loads back to the first scene in the build index
            SceneManager.LoadScene(0);
        }
    }
}
