using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScore : MonoBehaviour
{
    public TMP_Text timeText;
    public Image bronzeMedal;
    public Image silverMedal;
    public Image goldMedal;

    // Start is called before the first frame update
    void Start()
    {
        bronzeMedal.enabled = false;
        silverMedal.enabled = false;
        goldMedal.enabled = false;

        timeText.text ="Your finish time was: " +  PlayerTank.FinishTime.ToString();

        if(PlayerTank.FinishTime < 100)
        {
            goldMedal.enabled = true;
        }else if (PlayerTank.FinishTime < 130)
        {
            silverMedal.enabled = true;
        }
        else
        {
            bronzeMedal.enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
