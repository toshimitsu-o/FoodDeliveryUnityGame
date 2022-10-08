using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class timer : MonoBehaviour
{
    public static float currentTime;
    public int startMinutes;
    public TMP_Text currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {
        // Update timer
        currentTime = currentTime + Time.deltaTime;
        int time = (int) currentTime; // Converting to Int
        // Set the time to Text
        currentTimeText.text = "Time: " + time.ToString();
    }

   
}
