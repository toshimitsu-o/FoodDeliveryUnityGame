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

	}
	

	/*
	 * Attack state
	 */
    protected void UpdateAttackState() {
    }


    /*
     * Dead state
     */
    protected void UpdateDeadState() {
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

}