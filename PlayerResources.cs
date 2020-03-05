using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {
    AudioSource source;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void coinCollected()
    {
        source.Play();
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1);
        Debug.Log(getMoney());
    }
    public int getMoney() { return PlayerPrefs.GetInt("coins"); }
    public void setMoney(int money_) { PlayerPrefs.SetInt("coins", money_); }
}
