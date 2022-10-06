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

	public TMP_Text healthText;


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

		healthText.text = "Health: " + playerHealth;

		if(playerHealth == 0)
        {
			SceneManager.LoadScene("NPC Testing");
        }
	}

	public void RestartGame() {
		SceneManager.LoadScene("Week04");
	}

    public void ApplyDamage()
    {
        playerHealth = playerHealth - 25;
        healthFloat = (float)playerHealth;
        healthPercentage = healthFloat / 100f;
        print(healthPercentage);

        healthBar.rectTransform.sizeDelta = new Vector2(OriginalHealthX * healthPercentage, healthBarHeight);

    }
    public void ApplyHealing()
    {
        playerHealth = playerHealth + 25;
        healthFloat = (float)playerHealth;
        healthPercentage = healthFloat / 100f;
        print(healthPercentage);

        healthBar.rectTransform.sizeDelta = new Vector2(OriginalHealthX * healthPercentage, healthBarHeight);

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Slowdown")
        {
			moveSpeed = moveSpeed / 2;
        }

        if (other.tag == "Chicken")
        {
            ApplyDamage();
            Destroy(other.gameObject);
        }

        if (other.tag == "UFO")
        {
			PlayerTransform.position = UFO_Waypoint.position;

		}
        if (other.gameObject.tag == "Health")
        {
            if (playerHealth >= 100)
            {
                Destroy(other.gameObject);
            }
            else if (playerHealth < 100)
            {
                ApplyHealing();
                Destroy(other.gameObject);
            }

        }
        // When player collide with goal object load the end scene
        if(other.gameObject.tag == "Goalpickup")
        {
            SceneManager.LoadScene("EndScreen");
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

}
