using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour
{
    public PlayerTank script;
    public Text speedLabel;
    // Start is called before the first frame update
    void Start(){ 
        speedLabel.text = "Speed: " + script.moveSpeed.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (script.isboosted == true){
            speedLabel.text = "Speed: " + script.moveSpeed.ToString();
        }
    }
}
