using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SimpleFSM : MonoBehaviour 
{
    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    // List of waypoints
    public GameObject[] waypointList;

    // reference the NavMeshAgent
    private NavMeshAgent nav;
    private int currentWaypoint;

    // Current state that the NPC is reaching
    public FSMState curState;

	protected Transform playerTransform;// Player Transform

	// Turret
	public GameObject turret;
	public float turretRotSpeed = 4.0f;
	
    // Bullet
	public GameObject bullet;
	public GameObject bulletSpawnPoint;

	// Bullet shooting rate
	public float shootRate = 3.0f;
	protected float elapsedTime;

    // Whether the NPC is destroyed or not
    protected bool bDead;
    public int health = 100;

	// Ranges for chase and attack
	public float chaseRange = 35.0f;
	public float attackRange = 20.0f;
	public float attackRangeStop = 10.0f;

	public GameObject explosion;
	public GameObject smokeTrail;

    // State changed to Patrol or not
    private bool outOfPatrol = false;

    /*
     * Initialize the Finite state machine for the NPC tank
     */
	void Start() {

        nav = GetComponent<NavMeshAgent>();
        currentWaypoint = 0;
        nav.SetDestination(waypointList[currentWaypoint].transform.position);

        curState = FSMState.Patrol;

        bDead = false;
        elapsedTime = 0.0f;

        // Get the target enemy(Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if(!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");
	}


    // Update each frame
    void Update() {
        switch (curState) {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        // Update the time
        elapsedTime += Time.deltaTime;

        // Go to dead state if no health left
        if (health <= 0)
            curState = FSMState.Dead;

        //Debug.Log(curState);
    }

	/*
     * Patrol state
     */
    protected void UpdatePatrolState() {
        //Debug.Log("Patroling");
        //Debug.Log(Vector3.Distance(transform.position, waypointList[currentWaypoint].transform.position));

        // Check if the state just changed to Patrol from others
        if (outOfPatrol) {
            nav.SetDestination(waypointList[currentWaypoint].transform.position);
            outOfPatrol = false;
        }

        // NavMeshAgent move code goes here (Task 1.1)
        if (Vector3.Distance(transform.position, waypointList[currentWaypoint].transform.position) < 0.5)
        {
            if (currentWaypoint >= waypointList.Length -1)
            {
                currentWaypoint = 0;
            } else
            {
                currentWaypoint += 1;
            }
            nav.SetDestination(waypointList[currentWaypoint].transform.position);
        }

        // Transitions
        // Check the distance with player tank
        // When the distance is near, transition to chase state
        if (Vector3.Distance(transform.position, playerTransform.position) <= chaseRange) {
            outOfPatrol = true;
            curState = FSMState.Chase;
        }
    }

    private void SetDestToPlayer() {
        if(System.DateTime.Now.Second % 0.25 == 0)
            nav.SetDestination(playerTransform.position);
    }

    /*
     * Chase state
	 */
    protected void UpdateChaseState() {

		// NavMeshAgent move code goes here (Task 1.2)
        SetDestToPlayer();

		// Transitions
        // Check the distance with player tank
        // When the distance is near, transition to attack state
		float dist = Vector3.Distance(transform.position, playerTransform.position);
		if (dist <= attackRange) {
            curState = FSMState.Attack;
        }
        // Go back to patrol is it become too far
        else if (dist >= chaseRange) {
            curState = FSMState.Patrol;
		}
	}
	

	/*
	 * Attack state
	 */
    protected void UpdateAttackState() {

		// Transitions
		// Check the distance with the player tank
        float dist = Vector3.Distance(transform.position, playerTransform.position);
		if (dist > attackRange) {
			curState = FSMState.Chase;
		}
        // Transition to patrol if the tank is too far
        else if (dist >= chaseRange) {
			curState = FSMState.Patrol;
		} else if (dist <= attackRangeStop) { // If in attackRangeStop range
            nav.isStopped = true;
        } else {
            SetDestToPlayer(); // Set destination
            nav.isStopped = false; // move
        }

        // Always Turn the turret towards the player
		if (turret) {
			Quaternion turretRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
        	turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, turretRotation, Time.deltaTime * turretRotSpeed); 
		}

        // Shoot the bullets
        ShootBullet();
    }


    /*
     * Dead state
     */
    protected void UpdateDeadState() {
        // Show the dead animation with some physics effects
        if (!bDead) {
            nav.enabled = false; // Stop the navigation
            bDead = true;
            Explode();
        }
    }


    /*
     * Shoot Bullet
     */
    private void ShootBullet() {
        if (elapsedTime >= shootRate) {
			if ((bulletSpawnPoint) & (bullet)) {
            	// Shoot the bullet
            	Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
			}
            elapsedTime = 0.0f;
        }
    }

    // Apply Damage if hit by bullet
    public void ApplyDamage(int damage ) {
    	health -= damage;
    }


    protected void Explode() {
        float rndX = Random.Range(8.0f, 12.0f);
        float rndZ = Random.Range(8.0f, 12.0f);
        for (int i = 0; i < 3; i++) {
            GetComponent<Rigidbody>().AddExplosionForce(10.0f, transform.position - new Vector3(rndX, 2.0f, rndZ), 45.0f, 40.0f);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 10.0f, rndZ));
        }

		if (smokeTrail) {
			GameObject clone = Instantiate(smokeTrail, transform.position, transform.rotation) as GameObject;
			clone.transform.parent = transform;
		}
		Invoke ("CreateFinalExplosion", 1.4f);
		Destroy(gameObject, 1.5f);
	}
	
	
	protected void CreateFinalExplosion() {
		if (explosion) 
			Instantiate(explosion, transform.position, transform.rotation);
	}


	void OnDrawGizmos () {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, chaseRange);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}

}
