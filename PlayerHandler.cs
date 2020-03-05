using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour {
    Animator animator;
    PlayerResources resources;
    UIHandler UI;
    PlayerCombat combat;
    CorgiController controller;
    public GameObject pauseScreen;
    GameObject pscreen;
    bool pause;
    public GameObject vendingScreen;
    GameObject vscreen;
    bool canbuy; 

    string state;
    bool left;
	// Use this for initialization
	void Awake () {
        animator = this.gameObject.GetComponent<Animator>();
        resources = this.gameObject.GetComponent<PlayerResources>();
        UI = this.gameObject.GetComponent<UIHandler>();
        combat = this.gameObject.GetComponent<PlayerCombat>();
        controller = this.gameObject.GetComponent<CorgiController>();
        state = "Idle";
        left = false;
        vscreen = Instantiate(vendingScreen) as GameObject;
        vscreen.SetActive(false);
        canbuy = false;
        pscreen = Instantiate(pauseScreen) as GameObject;
        pscreen.SetActive(false);
        pause = false;
    }

    public void buyItem(int combo)
    {
        Debug.Log("atempting to buy " + combo);
        int price = 0;
        if (combo == 4) price = 3;
        else if (combo == 3) price = 10;
        else if (combo == 1) price = 15;
        resources = GetComponent<PlayerResources>();
        Debug.Log("Price: " + price + ", money: " + resources.getMoney());
        resources = GetComponent<PlayerResources>();
        combat = GetComponent<PlayerCombat>();
        if(PlayerPrefs.GetInt(combo.ToString()) != 0)
        {
            Debug.Log("already owned");
            return;
        }
        else if (price <= resources.getMoney() )
        {
            Debug.Log("buying " + combo);
            int y = resources.getMoney();
            resources.setMoney(y - price);
            combat.unlockCombo(combo);
            flashbought();
            return;
        }
        else Debug.Log("notenough"); flashnotenough(); return;
    }
    // Update is called once per frame
    void Update () {
        //Input for attack
        checkAttack();
        //animation
        float x = Input.GetAxisRaw("Player1_Horizontal");
        if (state == "Idle")
        {
            if (x != 0/*Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)*/ && !combat.getAttacking())
            {
                animator.SetTrigger("Running"); state = "Running";
            }
        }
        else if (state == "Running")
        {
            if (x == 0/*!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)*/)
            {
                animator.SetTrigger("Idle"); state = "Idle";
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
        }
        
        if (x < 0) { left = true; }
        if(x > 0) { left = false; }

        UI.updateScore(this.gameObject.GetComponent<Health>().CurrentHealth,resources.getMoney());

        if(canbuy && Input.GetKeyDown(KeyCode.Mouse0) && !vendingScreen.activeSelf && !pscreen.activeSelf)
        {
            openShop();


        }
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "shop")
        {
            canbuy = true;
        }
        if (collision.gameObject.tag == "coin")
        {
            resources.coinCollected();
            Destroy(collision.gameObject);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "shop")
        {
            canbuy = false;
            
            if (vscreen.activeSelf)
            {
                close();
            }
        }

    } 
    public void checkAttack()
    {
        if (Input.GetButtonDown("QuickAttack"))
        {
            combat.startattack(left, 0);


        }
        else if (Input.GetButtonDown("StrongAttack"))
        {
            combat.startattack(left, 1);

        }
    }
    public void closeShop()
    {
        if(vscreen == null)
        {
            vscreen = GameObject.FindGameObjectWithTag("vscreen");
        }
        vscreen.SetActive(false);
    }
    public void togglePause()
    {
        if (!pause && !vendingScreen.activeSelf)
        {
            pscreen.SetActive(true);
            //Time.timeScale = 0;
            pause = true;
        }
        else if(pause)
        {
            //Time.timeScale = 1;
            pscreen.gameObject.SetActive(false);
            pause = false;

        }
    } 
    public void flashnotenough()
    {
        GameObject txt = GameObject.FindGameObjectWithTag("vscreen").transform.Find("notenough").gameObject;
        Debug.Log(txt == null);
        Color col = txt.GetComponent<Text>().color;
        txt.GetComponent<Text>().text = "Not Enough Money";
        col.a = 1;
        txt.GetComponent<Text>().color = col;
    }
    public void flashbought()
    {
        GameObject txt = GameObject.FindGameObjectWithTag("vscreen").transform.Find("notenough").gameObject;
        Color col = txt.GetComponent<Text>().color;
        Debug.Log(txt == null);
        txt.GetComponent<Text>().text = "Purchase Complete";
        col.a = 1;
        txt.GetComponent<Text>().color = col;
    }
    public void close()
    {
        closeShop();
    }
    public void openShop()
    {
        vscreen.SetActive(true);
        
    }  
}
