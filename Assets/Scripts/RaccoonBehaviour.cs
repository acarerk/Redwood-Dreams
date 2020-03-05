using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class RaccoonBehaviour : MonoBehaviour {
    string state;
    Color original;
    float prepTime;
    float prevLoc;
    CorgiController controller;
    float time;
    float cooldown;

    float range;
    CharacterHorizontalMovement movement;
    DamageOnTouch damTouch;
    // Use this for initialization
    void Start () {
        state = "idle";
        original = this.gameObject.GetComponent<SpriteRenderer>().color;
        prepTime = 1.5f;
        prevLoc = transform.position.x;
        controller = GetComponent<CorgiController>();
        time = 0;
        cooldown = 3;
        range = 3;
        movement = GetComponent<CharacterHorizontalMovement>();
        damTouch = GetComponent<DamageOnTouch>();
        damTouch.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(state == "idle" && movement.MovementSpeed != 1)
        {
            movement.MovementSpeed = 1;
        }
        //position and direction to spawn raycast
        time += Time.deltaTime;
        Vector2 pos = this.transform.position;
        Vector2 direction = -Vector2.left;
        float x = (transform.position.x - prevLoc) / Time.deltaTime;
        prevLoc = transform.position.x;
        if (x > 0)
        {
            direction = Vector2.right;
            pos.x += 0.5f;
        }
        if(x < 0)
        {
            direction = Vector2.left;
            pos.x -= 0.5f;
        }
        if(time > cooldown && !controller.enabled)
        {
            controller.enabled = true;
            movement.WalkSpeed = 1;
            state = "idle";
        }

        //spawn raycast
        RaycastHit2D hit = Physics2D.Raycast(pos, direction, range);
        
        Debug.DrawRay(pos, direction * range, Color.green);
        //check if hit
        if (hit && state == "idle" && time > cooldown)
        {
            
            if(hit.transform.gameObject.tag == "Player" && state == "idle")
            {
                Debug.Log("Hit player");
                time = 0;
                StartCoroutine(attack(direction));
                
            }
        }
    }
    IEnumerator attack(Vector2 direction)
    {
        Debug.Log("in attack");
        state = "attacking";
        controller.enabled = false;

        //change color to show preperation
        Color c = this.gameObject.GetComponent<SpriteRenderer>().color;
        c = Color.green;
        this.gameObject.GetComponent<SpriteRenderer>().color = c;
        yield return new WaitForSeconds(prepTime);
        this.gameObject.GetComponent<SpriteRenderer>().color = original;
        controller.enabled = true;
        damTouch.enabled = true;
        movement.MovementSpeed = 15;
        yield return new WaitForSeconds(0.3f);
        state = "idle";
        movement.MovementSpeed = 1;
        damTouch.enabled = false;
        controller.enabled = false;

        
    }
    public void interruptAttack()
    {
        if (state == "attacking")
        {
            StopCoroutine("attack");
            state = "idle";
            controller.enabled = false;

        }
        
    }
    public IEnumerator stun()
    {
        yield return new WaitForSeconds(1.5f);
        controller.enabled = true;
        damTouch.enabled = false;
    }
}
