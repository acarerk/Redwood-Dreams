using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    // Use this for initialization
    GameObject coinText;
	void Start () {
        coinText = GameObject.Find("CoinText");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void updateScore(int hearts, int money)
    {
        coinText.GetComponent<Text>().text = "" + money;
    }
}
