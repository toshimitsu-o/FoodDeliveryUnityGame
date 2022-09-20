using UnityEngine;
using System.Collections;

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

    public GameObject[] waypointList; 
    private UnityEngine.AI.NavMeshAgent nav;
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
    protected Vector3 destPos;
    public float moveSpeed = 12.0f; // Speed of the tank
    public float rotSpeed = 2.0f; // Tank Rotation Speed

    /*
     * Initialize the Finite state machine for the NPC tank
     */
	void Start() {

        curState = FSMState.Patrol;

        bDead = false;
        elapsedTime = 0.0f;

        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

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
    }

	/*
     * Patrol state
     */
    protected void UpdatePatrolState() {
        //print("changed to patrol state");
        ShootBullet();
    }


    /*
     * Chase state
	 */
    protected void UpdateChaseState() {

		// NavMeshAgent move code goes here
        print("changed to chase state");
        Vector3 chaserPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
		Vector3 targetPos = new Vector3(playerTransform.position.x, 0.0f, playerTransform.position.z);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - chaserPos), Time.deltaTime * rotSpeed));
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * moveSpeed);

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
		}

        // Always Turn the turret towards the player
		if (turret) {
			Quaternion turretRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
        	turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, turretRotation, Time.deltaTime * turretRotSpeed); 
		}

        if (dist <= attackRangeStop){
            nav.isStopped = true;
        }
        else if (dist>= attackRangeStop){
            nav.isStopped = false;
        }

        // Shoot the bullets
        ShootBullet();
    }


    /*
     * Dead state
     */
    protected void UpdateDeadState() {
        nav.enabled = false;
        // Show the dead animation with some physics effects
        if (!bDead) {
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

}
