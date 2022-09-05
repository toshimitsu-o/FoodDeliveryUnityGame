using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_FSM : MonoBehaviour
{

    public enum FSMState
    {
        None,
        Race_Start,
        Race_Finish,
    }
    public GameObject waypoint;
    public FSMState curState;
    private NavMeshAgent nav;


    // Start is called before the first frame update
    void Start()
    {
        curState = FSMState.Race_Start;
        nav = GetComponent<NavMeshAgent>();
        
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
        nav.SetDestination(waypoint.transform.position);
    }

   
}
