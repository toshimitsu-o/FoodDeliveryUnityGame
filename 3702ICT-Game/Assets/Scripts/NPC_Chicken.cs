using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Chicken : MonoBehaviour
{

    public GameObject[] waypointList;
    private NavMeshAgent nav;
    public int patrolCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(waypointList[patrolCount].transform.position);
        float dist = Vector3.Distance(transform.position, waypointList[patrolCount].transform.position);
       
       

        if (dist <= 2.3)//<- Distance from waypoint
        {
            
            patrolCount = patrolCount + 1;
            
            if (patrolCount == 2 && dist <= 2.3)
            {
                patrolCount = 0;
            }
        }

    }

 
}
