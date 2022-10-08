using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Bus : MonoBehaviour
{
    public GameObject[] waypointList; // List of waypoints for patrolling
    private NavMeshAgent nav;
    private int curWaypoint = 0;


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(waypointList[curWaypoint].transform.position);
        float dist = Vector3.Distance(transform.position, waypointList[curWaypoint].transform.position);
       
        if (dist <= 5)//<- Distance from waypoint
        {

            curWaypoint = curWaypoint + 1;

            if (curWaypoint == 6 && dist <= 5)
            {
                curWaypoint = 0;
            }
        }
    }
}
