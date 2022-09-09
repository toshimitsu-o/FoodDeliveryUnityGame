using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinpickup : MonoBehaviour
{
    //private PlayerTank playertank;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider playerTransform) {
        // do something
        if (playerTransform.gameObject.tag == "Player"){
            playerTransform.gameObject.SendMessage("ApplyAmmoPickup");
            Destroy(gameObject);
        }
    }
}
