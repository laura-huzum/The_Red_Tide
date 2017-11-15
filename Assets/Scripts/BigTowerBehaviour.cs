using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTowerBehaviour : GenericWeapon
{


    bool isColliding;


    private TimeSpan last_shot;
    private TimeSpan interval = new TimeSpan(0, 0, 0, 0, 3000); // 3000 ms
    private List<GameObject> enemyList;
    private double seconds_oldest_target;
    private GameObject target;

    GameManagerBehavior gameManager;
    bool toggle_collider = false;


    void Start()
    {
        seconds_oldest_target = 0;

        enemyList = new List<GameObject>();

    }

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        shooting_state = false;
        gameObject.layer = LayerMask.NameToLayer("HoverOver");
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




    private void OnMouseUp()
    {


        if (!shooting_state && !isColliding)
        {
            // placed object
            shooting_state = true;
            gameObject.layer = LayerMask.NameToLayer("Default");
    
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision entered");
        isColliding = true;

        /*if (collision.collider.CompareTag("DefensiveStructure"))
            Debug.Log("colliding with another building");*/
        // Debug.Log(collision.collider.gameObject.name);
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
            // check tag for tanks too
            // check that collision is on the box collider
            if (collision.collider.gameObject.CompareTag("EnemyTank"))
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
        }
        else
        {

            enemyList.Remove(collision.collider.gameObject);//(collision.otherCollider.transform.parent.gameObject);
            //Debug.Log("Enemy list remove " + enemyList.Count);
        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {

        if (!gameManager.GetComponent<GameManagerBehavior>().gameOver)
        {
            if (shooting_state && enemyList.Count >= 1)
            {
                //get array of enemies
                //collision.otherCollider.transform.parent.gameObject;

                update_oldest_enemy();

                if (DateTime.Now.TimeOfDay - last_shot > interval)
                {

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
        seconds_oldest_target = 0;
        DateTime now = DateTime.Now;

        for (int i = 0; i < enemyList.Count; i++)
        {
            //Debug.Log("i=" + i);
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
                //Debug.Log("Enemy list remove " + enemyList.Count);
                i--;
            }
            else
            {
                GenericEnemy script = enemyList[i].GetComponent<GenericEnemy>();
                //Debug.Log(script);
                double enemy_age_seconds = (now - script.spawnTime).TotalMilliseconds;
                if (enemy_age_seconds > seconds_oldest_target)
                {
                    seconds_oldest_target = enemy_age_seconds;
                    target = enemyList[i];
                }
            }
        }
    }

}
