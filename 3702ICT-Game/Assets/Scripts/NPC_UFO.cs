using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NPC_UFO : MonoBehaviour
{
    public enum FSMState
    {
        None,
        Patrol,
        Chase,
       
    }

    // Current state that the NPC is reaching
    public FSMState curState;

    protected Transform playerTransform;// Player Transform

    public GameObject[] waypointList; // List of waypoints for patrolling

    public float chaseRange = 35.0f; //Range for chase

    private NavMeshAgent nav;

    private GameObject objPlayer;

    private int curWaypoint = -1;
    private bool setDest = false;


    // Start is called before the first frame update
    void Start()
    {
        curState = FSMState.Patrol;

        objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        nav = GetComponent<NavMeshAgent>();

        if (waypointList.Length > 0)
            curWaypoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            
        }

    }

    protected void UpdatePatrolState()
    {
        if (curWaypoint > -1)
        {
            print(Vector3.Distance(transform.position, waypointList[curWaypoint].gameObject.transform.position));
            // check if close to current waypoint
            if (Vector3.Distance(transform.position, waypointList[curWaypoint].gameObject.transform.position) <= 10.5f)
            {
                // get next waypoint
                curWaypoint++;
                // if we have travelled to last waypoint, go back to the first
                if (curWaypoint > (waypointList.Length - 1))
                    curWaypoint = 0;

                setDest = false;
            }

            if (!setDest)
            {
                // NavMeshAgent move
                nav.SetDestination(waypointList[curWaypoint].gameObject.transform.position);
                setDest = true;
            }

           
        }

        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist <= chaseRange)
        {
            curState = FSMState.Chase;
        }
    }
    protected void UpdateChaseState()
    {
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        nav.SetDestination(playerTransform.position);

        if (dist >= chaseRange)
        {
            curState = FSMState.Patrol;
            setDest = false;
        }
        
    }
}
