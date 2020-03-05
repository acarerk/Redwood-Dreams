using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void quit()
    {
        Application.Quit();
    }
    public void eraseSave()
    {
        PlayerPrefs.DeleteAll();
    }
    public void StartGame()
    {
        LoadingSceneManager.LoadScene("Tutorial");
    }
}
