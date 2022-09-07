using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Kangaroo : MonoBehaviour
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

       

        if (dist <= 1.4)//<- number might change when asset is applied
        {
            
            patrolCount = patrolCount + 1;
            print(patrolCount);
            if (patrolCount == 2 && dist <= 1.4)
            {
                patrolCount = 0;
            }
        }

    }

 
}
