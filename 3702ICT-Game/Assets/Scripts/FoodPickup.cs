using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickup : MonoBehaviour
{
    // Rotation amount
    public float rotateSpeed = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate Food Y-axis
        transform.Rotate(0.0f, rotateSpeed * Time.deltaTime, 0.0f, Space.Self);
        
    }

    // Ammo_Pickup disappear if the Player walks in
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.SendMessage("ApplyFoodPickup");
            Debug.Log("Food Cllided");
            Destroy(gameObject);
        } 
    }
}
