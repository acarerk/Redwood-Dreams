using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class PlayerCombat : MonoBehaviour {
    //variables
    float comboResetTime;
    float combotimer;
    float time;
    float cooldown;

    float knockback;

    bool interrupt;


    bool attacking = false;

    //prefabs
    public GameObject slash;
    public GameObject slashBig;

    bool waitdone;

    int quickAttackDamage;
    int strongAttackDamage;

    float quickAttackCooldown;
    float strongAttackCooldown;

    float quickAttackPrepTime;
    float strongAttackPrepTime;

    float quickAttackRecovTime;
    float strongAttackRecovTime;

    float quickAttackRange;
    float strongAttackRange;

    float comboRecoverTime;
    float comboRange;
    int combo1dmg;
    int combo2dmg;
    int combo3dmg;
    int combo4dmg;

    //combo ints 1==quickAttack 2=strongAttack 0=empty
    List<int> playerCombo;
    List<int> combo1;
    List<int> combo2;
    List<int> combo3;
    List<int> combo4;
    List<List<int>> combos;
    Color original;

    //start
	void Awake () {
        // setting up variables
        combotimer = 0;
        time = 0;
        cooldown = 0;
        comboResetTime = 1f;
        knockback = 0;

        quickAttackDamage = 1;
        quickAttackCooldown = 0.1f;

        strongAttackCooldown = 0.3f;
        strongAttackDamage = 2;

        quickAttackPrepTime = 0f;
        strongAttackPrepTime = 0f;

        quickAttackRecovTime = 0.1f;
        strongAttackRecovTime = 0.3f;

        quickAttackRange = 0.4f;
        strongAttackRange = 0.55f;

        comboRecoverTime = 0.5f;
        combo3dmg = 4;
        combo1dmg = 2;
        combo2dmg = 2;
        combo4dmg = 2;
        comboRange = 0.65f;

        interrupt = false;
        //set up combos
        setUpCombos();
        original = this.gameObject.GetComponent<SpriteRenderer>().color;
	}
	
    //update
	void Update () {
        //increment time
        time += Time.deltaTime;
        combotimer += Time.deltaTime;
        if(combotimer > comboResetTime)
        {
            comboReset();
            Debug.Log("combo reset");
            combotimer = 0;
            
        }
	}
    public void startattack(bool left, int type)
    {
        StartCoroutine( attack(left, type));
    } 
    
    void comboReset()
    {
        List<int> a = new List<int>();
        playerCombo = a;
    }
    //checks if there is a combo
    int comboCheck(List<int> playerCombo, List<List<int>> combos)
    {
        if (playerCombo.Count > 2 && combos.Count != 0)
        {
            Debug.Log(playerCombo[0] + " " + playerCombo[1] + " " + playerCombo[2]);

            for (int i = 0; i < combos.Count; i++)
            {
                if (playerCombo[0] == combos[i][0] && playerCombo[1] == combos[i][1] && playerCombo[2] == combos[i][2])
                {
                    return i + 1;
                }
            }
            return 0;
        }
        return 0;
    }
 
    public IEnumerator attack(bool left, int type)
    {
        Vector2 direction;
        float fallspeed = this.GetComponent<CorgiController>().DefaultParameters.FallMultiplier;
        //direction
        if (left) { direction = Vector2.left; } else { direction = -Vector2.left; }

        float recovTime;
        int damage;
        float range;
        float prepTime;
        float attackCooldown;
        GameObject slashpref;

        if (type == 0)
        {
            //Debug.Log("quickAttack");
            recovTime = quickAttackRecovTime;
            attackCooldown = quickAttackCooldown;
            range = quickAttackRange;
            prepTime = quickAttackPrepTime;
            damage = quickAttackDamage;
            slashpref = slash;
        }else
        {
            recovTime = strongAttackRecovTime;
            attackCooldown = strongAttackCooldown;
            range = strongAttackRange;
            prepTime = strongAttackPrepTime;
            damage = strongAttackDamage;
            slashpref = slashBig;
        }
        //check combos


        if (time > cooldown && attacking == false)
        {
            this.GetComponent<CorgiController>().DefaultParameters.FallMultiplier = 0;
            attacking = true;
            playerCombo.Add(type);

            int comb = comboCheck(playerCombo, combos);

            if (comb != 0)
            {
                recovTime = comboRecoverTime;
                Debug.Log("Combo " + comb);
                range = comboRange;
                playerCombo = new List<int>();
                if (comb == 3)
                {
                    knockback = 30; 
                    damage = combo3dmg;
                }else if (comb == 1)
                {
                    interrupt = true;
                    damage = combo1dmg;
                }else if(comb == 4)
                {
                    damage = combo4dmg;
                }

            }
            else if (playerCombo.Count >= 3)
            {
                playerCombo = new List<int>();
                recovTime = comboRecoverTime;
            }
            combotimer = 0;
            time = 0;
            cooldown = attackCooldown;
            //change color to indicate preperation
            Color c = Color.blue;
            this.gameObject.GetComponent<SpriteRenderer>().color = c;

            //show preperation animation
            //stop for a few secs
            yield return new WaitForSeconds(prepTime);


            //restore color
            c = original;
            this.gameObject.GetComponent<SpriteRenderer>().color = c;

            //spawn slash
            GameObject sl = Instantiate(slashpref, this.gameObject.transform, true);
            Vector3 pos = sl.transform.position;


            //set direciton
            int dir;
            if (left) { dir = -1; } else { dir = 1; }

            //transform slash
            sl.transform.localScale = new Vector3(Mathf.Abs(sl.transform.localScale.x) * dir, sl.transform.localScale.y * 0.6f, transform.localScale.z);
            pos = new Vector3(transform.position.x + (0.5f * dir), transform.position.y, transform.position.z);
            sl.transform.position = pos;
            pos = transform.position;
            pos.y -= 0.3f;
            pos.x += 0.35f * dir;
            //Raycast to check if any enemy is hit
            RaycastHit2D hitdown = Physics2D.Raycast(pos, direction, range);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(pos.x, pos.y + 0.3f),direction,range);
            RaycastHit2D hitup = Physics2D.Raycast(new Vector2(pos.x, pos.y + 0.6f), direction, range);
            Debug.DrawRay(pos, direction, Color.green);
            Debug.DrawRay(new Vector2(pos.x, pos.y + 0.3f), direction, Color.green);
            Debug.DrawRay(new Vector2(pos.x, pos.y + 0.6f), direction, Color.green);

            RaycastHit2D[] rays = new RaycastHit2D[3];
            rays[0] = hitup;
            rays[1] = hit;
            rays[2] = hitdown;

            List<GameObject> objectsHit = new List<GameObject>();
            for (int i = 0; i < rays.Length; i++) {
                if (rays[i])
                {

                    if (rays[i].transform.gameObject.tag == "Enemy")
                    {
                        if (objectsHit.Count != 0)
                        {
                            bool found = false;
                            for (int x = 0; x < objectsHit.Count; x++)
                            {
                                if (objectsHit[x] == rays[i])
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                objectsHit.Add(rays[i].transform.gameObject);
                            }
                        } else { objectsHit.Add(rays[i].transform.gameObject); }

                    }

                }
            }
            //damage
            if (objectsHit.Count > 0) {
                for (int z = 0; z < objectsHit.Count; z++)
                {

  

                    objectsHit[z].GetComponent<Health>().Damage(damage, this.gameObject, 1, 0.1f);
                    if (objectsHit[z].layer != 8)
                    {
                        if (knockback != 0 && !objectsHit[z].name.StartsWith("Book"))
                        {
                            objectsHit[z].GetComponent<CorgiController>().AddHorizontalForce(knockback * dir);
                            knockback = 0;
                        }
                        RaccoonBehaviour rac = objectsHit[z].GetComponent<RaccoonBehaviour>();
                        if (interrupt && rac != null)
                        {
                            rac.interruptAttack();
                            interrupt = false;
                        }
                    }
                }
                combotimer = 0;
                //recover faster if attack hits
                //attackAnimation
            }
  
            this.gameObject.GetComponent<SpriteRenderer>().color = original;
            yield return new WaitForSeconds(recovTime);
            Destroy(sl);
            attacking = false;
            this.GetComponent<CorgiController>().DefaultParameters.FallMultiplier = fallspeed;
        }
    }
    public void setUpCombos()
    {
        playerCombo = new List<int>();
        combos = new List<List<int>>();

        //quick, quick, heavy
        combo1 = new List<int>();
        combo1.Add(0);
        combo1.Add(0);
        combo1.Add(1);

        //quick, heavy, quick
        combo2 = new List<int>();
        combo2.Add(0);
        combo2.Add(1);
        combo2.Add(0);

        //quick, heavy, heavy
        combo3 = new List<int>();
        combo3.Add(0);
        combo3.Add(1);
        combo3.Add(1);

        //quick, quick, quick
        combo4 = new List<int>();
        combo4.Add(0);
        combo4.Add(0);
        combo4.Add(0);

        if(PlayerPrefs.GetInt("1") == 1)
        {
            combos.Add(combo1);
        }
        if (PlayerPrefs.GetInt("2") == 1)
        {
            combos.Add(combo2);
        }
        if (PlayerPrefs.GetInt("3") == 1)
        {
            combos.Add(combo3);
        }
        if(PlayerPrefs.GetInt("4") == 1)
        {
            combos.Add(combo4);
        }

    }
    public void unlockCombo(int combo)
    {
        if(PlayerPrefs.GetInt(combo.ToString()) == 0)
        {
            PlayerPrefs.SetInt(combo.ToString(), 1);
            if(combo == 1)
            {
                combos.Add(combo1);
                //GameObject but = GameObject.Find("Combo3Button");
                //Destroy(but);
            }
            else if (combo == 2)
            {
                combos.Add(combo2);

            }
            else if (combo == 3)
            {
                combos.Add(combo3);
                //GameObject but = GameObject.Find("Combo2Button");
                //Destroy(but);
            }
            else if (combo == 4)
            {
                if(combos == null)
                {
                    combos = new List<List<int>>();
                }
                combos.Add(combo4);
                //GameObject but = GameObject.Find("Combo4Button");
                //Destroy(but);
            }
        }
    }
    public void setTime(int time_)
    {
        time = time_;
    }
    public void setAttacking(bool a)
    {
        attacking = a;
    }
    public bool getAttacking()
    {
        return attacking;
    }
}
