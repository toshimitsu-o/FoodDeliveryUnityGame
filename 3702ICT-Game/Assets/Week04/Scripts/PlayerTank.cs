using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerTank : MonoBehaviour
{

	public float moveSpeed = 100.0f;  // units per second
	public float rotateSpeed = 20.0f;

	private Transform _transform;
	private Rigidbody _rigidbody;


	// Use this for initialization
	void Start()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();
		rotateSpeed = rotateSpeed * 180 / Mathf.PI; // convert from rad to deg for rot function
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
	}

	public void RestartGame() {
		SceneManager.LoadScene("Week04");
	}

}
