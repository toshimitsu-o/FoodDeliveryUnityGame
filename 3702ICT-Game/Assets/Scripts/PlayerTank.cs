using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
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


	// Use this for initialization
	void Start()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();
		rotateSpeed = rotateSpeed * 180 / Mathf.PI; // convert from rad to deg for rot function

		PlayerTransform = GameObject.Find("Player").transform;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Slowdown")
        {
			moveSpeed = moveSpeed / 2;
        }
		
		if(other.tag == "Chicken")
        {
			playerHealth = playerHealth - 25;
        }

		if (other.tag == "UFO")
        {
			PlayerTransform.position = UFO_Waypoint.position;

		}
		if (other.gameObject.tag == "Health")
		{
			Destroy(other.gameObject);
			playerHealth = playerHealth + 25;
		}
    }
    private void OnTriggerExit(Collider other)
    {
		if (other.tag == "Slowdown")
		{
			moveSpeed = 10.0f;
		}
	}

}
