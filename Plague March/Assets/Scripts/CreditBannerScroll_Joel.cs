using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditBannerScroll_Joel : MonoBehaviour
{
    private Image bannerImg;
    public float scrollSpeedMultiplier;
    public Image tooltipImage;

    // Use this for initialization
    void Start()
    {
        bannerImg = GetComponent<Image>();
        tooltipImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bannerImg.transform.position.y <= 1700)
        {
            bannerImg.transform.Translate(new Vector3(0.0f, 1.0f * scrollSpeedMultiplier, 0.0f));
        }
        
        if(bannerImg.transform.position.y >= 1600)
        {
            tooltipImage.enabled = true;
        }

        if(tooltipImage.enabled == true && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}
