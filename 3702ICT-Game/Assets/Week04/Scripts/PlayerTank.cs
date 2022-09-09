using UnityEngine;
using System.Collections;

public class PlayerTank : MonoBehaviour {
	public float boosttimer;
	public bool isboosted = false;

	public float moveSpeed = 100.0f;  // units per second
	public float rotateSpeed = 3.0f;
	public float time = 1; 
	
	private Transform _transform;
	private Rigidbody _rigidbody;
	
	// Use this for initialization
	void Start () {
		print(isboosted);
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();
		rotateSpeed = rotateSpeed * 180 / Mathf.PI; // convert from rad to deg for rot function
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// check for input
		float rot = _transform.localEulerAngles.y + rotateSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
		Vector3 fwd = _transform.forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

		// Tank Chassis is rigidbody, use MoveRotation and MovePosition
		_rigidbody.MoveRotation(Quaternion.AngleAxis(rot, Vector3.up));
		_rigidbody.MovePosition(_rigidbody.position + fwd);
		print(boosttimer);

		if (isboosted == true){
			print(isboosted);
			moveSpeed = 150;
			boosttimer += time * Time.deltaTime;
			if (boosttimer >= 5){
				isboosted = false;
			}
		}
		if (isboosted == false){
			isboosted = false;
			moveSpeed = 100;
			boosttimer = 0;
		}


	// When Ammo get picked up, increase bullet count 
	}
	void ApplyAmmoPickup() {
		isboosted = true;
		//Debug.Log("Bullet count: " + bulletCount);
	}
	/*
	private IEnumerator speedDecrease(float waitTime){
		while (true){
			moveSpeed = 20.0f;
			yield return new WaitForSeconds(waitTime);
			
		}
	}*/

}
