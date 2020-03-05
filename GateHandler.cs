using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateHandler : MonoBehaviour {
    public List<GameObject> guards;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (guards.Count == 0)
        {
            Destroy(this.gameObject);
            return;
        } else {
            bool allempty = true;
            for (int i = 0; i < guards.Count; i++)
            {
                if (guards[i] != null)
                {
                    if (guards[i].activeSelf)
                    {
                        allempty = false;
                    }
                }
            }
            if (allempty) { Destroy(this.gameObject); }
        }
	}
}
