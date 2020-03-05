using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class Shooter : MonoBehaviour {
    public GameObject bullet;
    public GameObject player;
    Vector2 direction;
    bool inrange;
    float time;
    float cooldown;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        direction = Vector2.right;
        inrange = false;
        time = 0;
        cooldown = 2;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        time += Time.deltaTime;
        if (inrange && player != null && time > cooldown)
        {
            time = 0;
            direction = player.transform.position - this.transform.parent.transform.position;
            direction.Normalize();
            GameObject bul = Instantiate(bullet, this.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
            bul.GetComponent<Projectile>().Direction = direction;
        }
	}
    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            inrange = true;
        }
    }
}
