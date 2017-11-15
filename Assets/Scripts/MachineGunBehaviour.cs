using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineGunBehaviour : GenericWeapon
{

    GameObject structure;
    public GameObject structure_prefab;
  
    private GameManagerBehavior gameManager;

    bool isColliding;


    private TimeSpan last_shot;
    private TimeSpan interval = new TimeSpan(0,0,0,0,1500); // 1500 ms
    private List<GameObject> enemyList;
    
    private GameObject target;
    bool toggle_collider = false;


    // Use this for initialization
    // Start is called when the object becomes part of the scene
    void Start()
    {
        enemyList = new List<GameObject>();
        
    }

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        shooting_state = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (hitpoints <= 0)
            Destroy(gameObject);
        if (!shooting_state)
        {
            gameObject.transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                gameObject.transform.position.y,
                                                -0.1f);
        }
        else
        {
            // toggle the structure's circle collider corresponding to the phase of the game (placement or fight)
            if (gameManager.GetComponent<GameManagerBehavior>().stage_fight && !toggle_collider)
            {
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
                toggle_collider = true;
            }
            else if (!gameManager.GetComponent<GameManagerBehavior>().stage_fight && toggle_collider)
            {
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                toggle_collider = false;
            }
        }

    }

    public bool canUpgrade()
    {
        
        WeaponData weaponData = gameObject.GetComponent<WeaponData>();
        UpgradeLevel nextLevel = weaponData.getNextLevel();
        if (nextLevel != null && gameManager.Reichsmark > nextLevel.cost && !gameManager.stage_fight)
        {
            return true;
        }
        else return false;
        
    }


    private bool affordPlace()
    {

        int cost = gameObject.GetComponent<WeaponData>().levels[0].cost;
        return gameManager.Reichsmark >= cost;
    }


    /*public void PlaceAction()
    {
        structure = (GameObject) Instantiate(structure_prefab, Camera.main.ScreenToViewportPoint(Input.mousePosition), Quaternion.identity);
        
        GameManagerBehavior gameManagerTemp = structure.GetComponent<MachineGunBehaviour>().gameManager;
        int place_cost = structure.GetComponent<WeaponData>().levels[0].cost;


        if (gameManagerTemp.Reichsmark < place_cost)
        {
            // if cannot afford, destroy the object
            Destroy(structure);
            
        }
        else
        {
            // deduct gold
            structure.layer = LayerMask.NameToLayer("HoverOver");
            gameManagerTemp.Reichsmark -= place_cost;
        }
    }*/


    private void OnMouseUp()
    {

        
        if (!shooting_state && !isColliding)
        {
            // object can be placed on the position where the mouse is released
            shooting_state = true;
            gameObject.layer = LayerMask.NameToLayer("Default");
            // add to current game data

        }
        else
        {
            // check if the click is on box collider
            BoxCollider2D gun_base = gameObject.GetComponent<BoxCollider2D>();
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 down_left_corner = new Vector2(gameObject.transform.position.x - gun_base.size.x / 2,
                                                    gameObject.transform.position.y - gun_base.size.y / 2);
            Vector2 up_right_corner = new Vector2(gameObject.transform.position.x + gun_base.size.x / 2,
                                                    gameObject.transform.position.y + gun_base.size.y / 2);
            if (mousePosition.x > down_left_corner.x && mousePosition.y > down_left_corner.y &&
                mousePosition.x < up_right_corner.x && mousePosition.y < up_right_corner.y) {
                if (canUpgrade() && shooting_state)
                {
                    gameObject.GetComponent<WeaponData>().upgrade();
                    // deduct gold
                    gameManager.Reichsmark -= gameObject.GetComponent<WeaponData>().CurrentLevel.cost;
                }
           }
        }
    }




    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision entered");
        isColliding = true;

        // the transform member, although not initialized in the behavioural script,
        // is a member of the GameObject class, and it is tied to the GameObject to 
        // which the script is assigned.
        if (!shooting_state)
            foreach (Transform child in transform)
            {
                child.GetComponent<SpriteRenderer>().color = Color.red;
                //Debug.Log(child);
            }
        else
        {
            if (collision.collider.gameObject.CompareTag("EnemyInfantry"))
            {
                enemyList.Add(collision.collider.gameObject);
                //Debug.Log("Enemy list add " + enemyList.Count);
            }
        }

        //Debug.Log("COLLIDING BOYS");
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Collision exitted");
        if (!shooting_state)
        {
            isColliding = false;
            // turn back to default
            foreach (Transform child in transform)
            {
                child.GetComponent<SpriteRenderer>().color = Color.white;
            }
        } else
        {

            enemyList.Remove(collision.collider.gameObject);//(collision.otherCollider.transform.parent.gameObject);
            //Debug.Log("Enemy list remove " + enemyList.Count);
        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (! gameManager.GetComponent<GameManagerBehavior>().gameOver)
        {
            if (shooting_state && enemyList.Count >= 1)
            {
                //get array of enemies
                //collision.otherCollider.transform.parent.gameObject;

                update_oldest_enemy();
               

                if (DateTime.Now.TimeOfDay - last_shot > interval)
                {
                    //Debug.Log(target);
                    //Debug.Log("shooting at this old boy: " + seconds_oldest_target);
                    // play sound
                    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                    AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                    last_shot = DateTime.Now.TimeOfDay;

                    // remove HP
                    GenericEnemy script = target.GetComponent<GenericEnemy>();
                    // replace with damage
                    script.hitpoints -= damage;


                    Vector3 newStartPosition = gameObject.transform.position;
                    Vector3 newEndPosition = target.transform.position;
                    Vector3 newDirection = (newEndPosition - newStartPosition);
                    //2
                    float x = newDirection.x;
                    float y = newDirection.y;
                    float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
                    // get proper level sprite
                    GameObject current_level_sprite = gameObject.GetComponent<WeaponData>().CurrentLevel.visualization;
                    current_level_sprite.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

                }

            }
            else if (!shooting_state)
            {
                // still colliding with something
                isColliding = true;
                foreach (Transform child in transform)
                {
                    child.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
    }



    private void update_oldest_enemy()
    {
        double seconds_oldest_target = 0;
        DateTime now = DateTime.Now;
        bool haveDemoman = false;
        double enemy_age_seconds;
        GenericEnemy script;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
                i--;
            }
            else
            {
                script = enemyList[i].GetComponent<GenericEnemy>();
                enemy_age_seconds = (now - script.spawnTime).TotalMilliseconds;

                if (enemyList[i].name.Contains("Demo"))
                { //prioritize demoman
                    haveDemoman = true;
                    seconds_oldest_target = enemy_age_seconds;
                }

                //enemy_age_seconds = (now - script.spawnTime).TotalMilliseconds;
                if (!haveDemoman && enemy_age_seconds > seconds_oldest_target)
                {
                    seconds_oldest_target = enemy_age_seconds;
                    target = enemyList[i];
                }
                else if (haveDemoman && enemyList[i].name.Contains("Demo") && enemy_age_seconds >= seconds_oldest_target)
                {
                    seconds_oldest_target = enemy_age_seconds;
                    target = enemyList[i];
                }
            }
        }
    }


}
