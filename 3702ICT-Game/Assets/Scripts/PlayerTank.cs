using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerTank : MonoBehaviour
{

	public float moveSpeed = 10.0f;  // units per second
	public float rotateSpeed = 20.0f;
	public float playerHealth = 100;

	public Transform PlayerTransform;
	public Transform UFO_Waypoint;


	private Transform _transform;
	private Rigidbody _rigidbody;

    public float OriginalHealthX;
    public float healthBarHeight;
    public Image healthBar;
    public float healthPercentage;
    public float healthFloat;

    public TMP_Text orderStatusText; // Text for order status UI
    public int foodPickups = 0;
    public int foodPickupsMax = 3;
    public Mask foodMask; // Mask for food count UI

    public static float FinishTime;
    public string isboosted = "false";
    public float boosttimer;
    public float time = 1;

    // Sounds
    private AudioSource source;
    public AudioClip foodpickupFX;
    public AudioClip gasPickupFX;
    public AudioClip damageChickenFX;
    public AudioClip damageBusFX;
    public AudioClip damageCrowdFX;
    public AudioClip collideUfoFX;
    public AudioClip slowdownFX;
    public AudioClip emptyFX;
    public AudioClip collideFX;

    // Use this for initialization
    void Start()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();
		rotateSpeed = rotateSpeed * 180 / Mathf.PI; // convert from rad to deg for rot function

		PlayerTransform = GameObject.Find("Player").transform;
        PlayerTransform = GameObject.Find("Player").transform;
        OriginalHealthX = healthBar.sprite.rect.width;
        healthBarHeight = healthBar.rectTransform.rect.height;

        // Update Food counter UI
        UpdateFoodCount();
    }

	// Update is called once per frame
	void Update()
	{

		// check for input
		float rot = _transform.localEulerAngles.y + rotateSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
		Vector3 fwd = _transform.forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

		// Tank Chassis is rigidbody, use MoveRotation and MovePosition
		GetComponent<Rigidbody>().MoveRotation(Quaternion.AngleAxis(rot, Vector3.up));
		GetComponent<Rigidbody>().MovePosition(_rigidbody.position + fwd);

		//healthText.text = "Health: " + playerHealth;

		if(playerHealth <= 0)
        {
			SceneManager.LoadScene("LoseScreen");
        }
	}

	void Awake () {
        source = GetComponent<AudioSource>();
    }
    // Damage actions when collide
    public void ApplyDamage()
    {
        // Reduce health
        playerHealth = playerHealth - 25;
        healthFloat = (float)playerHealth;
        healthPercentage = healthFloat / 100f;
        print(healthPercentage);
        // Update health UI
        UpdateHealthUI();
        // Play FX sound
        source.PlayOneShot(damageChickenFX);
    }

    // Damage actions when collide with Crowd
    public void ApplyCrowdDamage()
    {
        // Reduce health
        playerHealth = playerHealth - 5;
        healthFloat = (float)playerHealth;
        healthPercentage = healthFloat / 100f;
        print(healthPercentage);
        // Update health UI
        UpdateHealthUI();

        // Play FX sound
        source.PlayOneShot(damageCrowdFX);
    }

    // Recover health points
    public void ApplyHealing()
    {
        // Add health
        playerHealth = playerHealth + 25;
        healthFloat = (float)playerHealth;
        healthPercentage = healthFloat / 100f;
        print(healthPercentage);
        // Update health UI
        UpdateHealthUI();
        // Play FX sound
        source.PlayOneShot(gasPickupFX);
    }

    public void ApplyBusDamage()
    {
        // Reduce health
        playerHealth = playerHealth - 50;
        healthFloat = (float)playerHealth;
        healthPercentage = healthFloat / 100f;
        print(healthPercentage);
        // Update health UI
        UpdateHealthUI();
        // Play FX sound
        source.PlayOneShot(damageBusFX);
    }

    // Update health monitor UI 
    private void UpdateHealthUI()
    {
        healthBar.rectTransform.sizeDelta = new Vector2(OriginalHealthX * healthPercentage, healthBarHeight);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Collide with slowdown (oil spill)
        if (other.tag == "Slowdown")
        {
			moveSpeed = moveSpeed / 2;
            // Play FX sound
            source.PlayOneShot(slowdownFX);
        }
        // Collide with Chicken
        if (other.tag == "Chicken")
        {
            ApplyDamage();

            Destroy(other.gameObject);
        }
        // Collide with UFO
        if (other.tag == "UFO")
        {
            // Teleport player
			PlayerTransform.position = UFO_Waypoint.position;
            // Play FX sound
            source.PlayOneShot(collideUfoFX);

		}
        // Collide with Bus
        if (other.tag == "Bus")
        {
            ApplyBusDamage();
        }
        // Collide with Health healer (Gas Stops)
        if (other.gameObject.tag == "Health")
        {
            if (playerHealth >= 100)
            {
                // Play FX sound
                source.PlayOneShot(emptyFX);
                //Destroy(other.gameObject);
            }
            else if (playerHealth < 100)
            {
                ApplyHealing();
                Destroy(other.gameObject);
            }

        }
        // When player collide with goal object load the end scene
        if (other.gameObject.tag == "Goalpickup")
        {
            SceneManager.LoadScene("EndScreen");
            FinishTime = timer.currentTime;
        }

        // When player collide with racer
        if (other.gameObject.tag == "Racer")
        {
            // Play FX sound
            source.PlayOneShot(collideFX);
        }

    }
    private void OnTriggerExit(Collider other)
    {
		if (other.tag == "Slowdown")
		{
			moveSpeed = 15.0f;
		}
	}

    // When collide with food, this gets triggered
    public void ApplyFoodPickup()
    {
        Debug.Log("Food picked up!");
        foodPickups += 1;
        // Update counter UI
        UpdateFoodCount();
        // Play FX sound
        source.PlayOneShot(foodpickupFX);
        // When reached to the max
        if (foodPickups >= foodPickupsMax)
        {
            //Debug.Log("All food picked up!");
            GameObject goal = GameObject.FindGameObjectWithTag("Goal");
            goal.SendMessage("ApplyFoodCollected");
        }
    }

    // Change the food mask size in UI
	public void UpdateFoodCount() {
        if (foodPickupsMax - foodPickups > 0) {
            orderStatusText.text = (foodPickupsMax - foodPickups).ToString() + " Orders to Collect";
        } else {
            orderStatusText.text = "Now deliver the food!";
        }
		if (foodMask) {
			RectTransform rectTransform = foodMask.GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(38f * foodPickups,rectTransform.sizeDelta.y);
	    }
	}

    public void ApplyCrowd() {
		isboosted = "crowd";
		//Debug.Log("Bullet count: " + bulletCount);
	}

}
