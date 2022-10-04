using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject goalPickup;
    public GameObject minimapGoalIcon;

    // Start is called before the first frame update
    void Start()
    {
        // Find pickup and icon
        goalPickup = GameObject.FindGameObjectWithTag("Goalpickup");
        minimapGoalIcon =  GameObject.FindGameObjectWithTag("MinimapGoalicon");
        // Hide them at start
        goalPickup.SetActive(false);
        minimapGoalIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When all food picked up, this gets triggered
    public void ApplyFoodCollected()
    {
        // Show the goal (icon for minimap as well)
        //Debug.Log("Goal shows up");
        goalPickup.SetActive(true);
        minimapGoalIcon.SetActive(true);
        
    }
}
