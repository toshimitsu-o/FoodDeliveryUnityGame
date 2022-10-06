﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerTank : MonoBehaviour
{

	public float moveSpeed = 15.0f;  // units per second
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

    public static float FinishTime;

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

		

		if(playerHealth == 0)
        {
            moveSpeed = 5.0f;
        }else if (playerHealth > 0)
        {
            moveSpeed = 15.0f;
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

    public void ApplyBusDamage()
    {
        playerHealth = playerHealth - 50;
        healthFloat = (float)playerHealth;
        healthPercentage = healthFloat / 100f;
        print(healthPercentage);

        healthBar.rectTransform.sizeDelta = new Vector2(OriginalHealthX * healthPercentage, healthBarHeight);

    }
    public void ApplyHealing()
    {
        playerHealth = playerHealth + 50;
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
        if(other.tag == "Bus")
        {
            ApplyBusDamage();
        }

        if(other.gameObject.tag == "Goal")
        {
            SceneManager.LoadScene("EndScreen");
            FinishTime = timer.currentTime;
        }

    }
    private void OnTriggerExit(Collider other)
    {
		if (other.tag == "Slowdown")
		{
			moveSpeed = 15.0f;
		}
	}

}
