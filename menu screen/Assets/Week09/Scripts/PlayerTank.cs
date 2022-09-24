using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PlayerTank : MonoBehaviour
{

	public float moveSpeed = 20.0f;  // units per second
	public float rotateSpeed = 3.0f;

	public int health = 100;
	private int maxHealth;
	private const int maxAmmo = 20;
	public int ammo = 10;

	//UI Variables
	public Image healthBar;
	private float healthXMax;
	public Text scoreText;
	public Mask ammoMask;
	private string score;

	private Transform _transform;
	private Rigidbody _rigidbody;

	public GameObject turret;
	public GameObject bullet;
	public GameObject bulletSpawnPoint;

	public float turretRotSpeed = 3.0f;

	//Bullet shooting rate
	public float shootRate = 1.5f;
	protected float elapsedTime;

	public Camera myCam;

	// Use this for initialization
	void Start()
	{
		healthXMax = healthBar.rectTransform.rect.width;
		maxHealth = health;

		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();

		elapsedTime = shootRate; // allow first press to fire
		rotateSpeed = rotateSpeed * 180 / Mathf.PI; // convert from rad to deg for rot function

		UpdateAmmoCount();
	}

    // Update is called once per frame
    void Update()
	{
		UpdatePlayerControls();
	}

	void UpdatePlayerControls()
    {
		// check for input
		float rot = _transform.localEulerAngles.y + rotateSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
		Vector3 fwd = _transform.forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

		// Tank Chassis is rigidbody, use MoveRotation and MovePosition
		GetComponent<Rigidbody>().MoveRotation(Quaternion.AngleAxis(rot, Vector3.up));
		GetComponent<Rigidbody>().MovePosition(_rigidbody.position + fwd);

		if (turret)
		{
			Plane playerPlane = new Plane(Vector3.up, transform.position + new Vector3(0, 0, 0));

			// Generate a ray from the cursor position
			Ray RayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Determine the point where the cursor ray intersects the plane.
			float HitDist = 0;

			// If the ray is parallel to the plane, Raycast will return false.
			if (playerPlane.Raycast(RayCast, out HitDist))
			{
				// Get the point along the ray that hits the calculated distance.
				Vector3 RayHitPoint = RayCast.GetPoint(HitDist);

				Quaternion targetRotation = Quaternion.LookRotation(RayHitPoint - transform.position);
				turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, targetRotation, Time.deltaTime * turretRotSpeed);
			}
		}

		if (Input.GetButton("Fire1"))
		{
			if (elapsedTime >= shootRate)
			{
				//Reset the time
				elapsedTime = 0.0f;

				if (ammo > 0)
				{
					//Also Instantiate over the PhotonNetwork
					if ((bulletSpawnPoint) & (bullet))
						Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
					ammo--;
				}
			}
			UpdateAmmoCount();
		}

		// Update the time
		elapsedTime += Time.deltaTime;
	}

	// Check the collision with the bullet
	void OnCollisionEnter(Collision collision)
	{
		// Reduce health
		if (collision.gameObject.tag == "Bullet")
			health -= collision.gameObject.GetComponent<Bullet>().damage;
	}

	void ApplyDamage(int dmg)
	{
		health -= dmg;
		healthBar.rectTransform.sizeDelta = new Vector2(((float)health / maxHealth) * healthXMax, healthBar.rectTransform.rect.height);
		//print ("HealthXMax is: " + healthXMax + ", healthbar width now is: " + ((float)health / maxHealth) * healthXMax);
	}
	public void ApplyAmmoPickup()
	{
		ammo = ammo + 5;
		if (ammo > maxAmmo) ammo = maxAmmo;
		UpdateAmmoCount();
	}

	public void UpdateScore(int addScore)
	{
		score = score + addScore;
		// update the GUI score here
		scoreText.text = "Score: " + score;
	}
	public void UpdateAmmoCount()
	{
		if (ammoMask)
		{
			RectTransform rectTransform = ammoMask.GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(13.0f * ammo, rectTransform.sizeDelta.y);
		}
	}


}
