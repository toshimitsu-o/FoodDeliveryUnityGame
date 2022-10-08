using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPC_FSM : MonoBehaviour
{

    public enum FSMState
    {
        None,
        Race_Start,
        Race_Finish,
    }
    //public GameObject waypoint;
    public FSMState curState;
    private NavMeshAgent nav;

    // Waypoints
    private int currentWaypoint;
    public GameObject[] waypointList;

    public TMP_Text enemyStatusText; // Text for enemy status UI


    // Start is called before the first frame update
    void Start()
    {
        curState = FSMState.Race_Start;
        nav = GetComponent<NavMeshAgent>();
        // Set the first waypoint
        currentWaypoint = 0;
        // Status text to set empty
        enemyStatusText.text = "0";
        
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (curState)
        {
            case FSMState.Race_Start: UpdateRaceStartState(); break;
            //case FSMState.Race_Finish: UpdateRaceFinishState(); break;
        }

    }

    protected void UpdateRaceStartState()
    {
        nav.SetDestination(waypointList[currentWaypoint].transform.position);

        // When NPC get to the current waypoint
        if (Vector3.Distance(transform.position, waypointList[currentWaypoint].transform.position) < 0.5)
        {
            if (currentWaypoint >= waypointList.Length -1)
            {
                Debug.Log("Goal!");
                enemyStatusText.text = "Finished!";
            } else
            {
                currentWaypoint += 1;
                enemyStatusText.text = currentWaypoint + " collected";
            }
            nav.SetDestination(waypointList[currentWaypoint].transform.position);
        }
    }

   
}
