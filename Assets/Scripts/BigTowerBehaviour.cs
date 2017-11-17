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
    bool toggle_collider = false;
    bool inRange = false;


    void Start()
    {
        seconds_oldest_target = 0;

        enemyList = new List<GameObject>();

    }

    private void Awake()
    {
        gm = GameObject.Find("GameManager");
        gmb = gm.GetComponent<GameManagerBehavior>();
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        //shooting_state = false;
        gameObject.layer = LayerMask.NameToLayer("HoverOver");
    }

    // Update is called once per frame
    void Update()
    {
        if (hitpoints <= 0)
            Destroy(gameObject);

        if (!shooting_state)
        {
            gameObject.transform.Find("range_indicator").gameObject.SetActive(true);
            gameObject.transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                gameObject.transform.position.y,
                                                -0.1f);
        }
        else
        {
            // toggle the structure's circle collider corresponding to the phase of the game (placement or fight)
            if (gmb.stage_fight && !toggle_collider)
            {
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
                toggle_collider = true;
            }
            else if (!gmb.stage_fight && toggle_collider)
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




    private void OnMouseOver()
    {
        BoxCollider2D gun_base = gameObject.GetComponent<BoxCollider2D>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 down_left_corner = new Vector2(gameObject.transform.position.x - gun_base.size.x / 2,
                                                gameObject.transform.position.y - gun_base.size.y / 2);
        Vector2 up_right_corner = new Vector2(gameObject.transform.position.x + gun_base.size.x / 2,
                                                gameObject.transform.position.y + gun_base.size.y / 2);
        if (mousePosition.x > down_left_corner.x && mousePosition.y > down_left_corner.y &&
            mousePosition.x < up_right_corner.x && mousePosition.y < up_right_corner.y)
        {
            // turn on the range indicator
            gameObject.transform.Find("range_indicator").gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        gameObject.transform.Find("range_indicator").gameObject.SetActive(false);
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
        else if (collision.collider.gameObject.CompareTag("EnemyTank"))
        {
            // check tag for tanks too

            // check that collision is on the box collider

            
            enemyList.Add(collision.collider.gameObject);
                

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
            inRange = false;
            if (enemyList.Count > 0)
                enemyList.Remove(collision.collider.gameObject);//(collision.otherCollider.transform.parent.gameObject);
            //Debug.Log("Enemy list remove " + enemyList.Count);
        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {

        if (!gmb.gameOver)
        {
            
            if (shooting_state && enemyList.Count >= 1)
            {
                //get array of enemies
                //collision.otherCollider.transform.parent.gameObject;
                
                
                
                update_oldest_enemy();
                double dist;
                float x1, x2, y1, y2;
                x1 = target.transform.position.x;
                y1 = target.transform.position.y;
                x2 = collision.otherCollider.gameObject.transform.position.x;
                y2 = collision.otherCollider.gameObject.transform.position.y;
                dist = Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2));

                //Debug.Log(dist);
                //dist = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
                //Debug.Log(dist);
                //Debug.Log("radius = " + collision.otherCollider.gameObject.GetComponent<CircleCollider2D>().radius);
                if (dist < collision.otherCollider.gameObject.GetComponent<CircleCollider2D>().radius)
                    inRange = true;
                else
                    inRange = false;

                if (DateTime.Now.TimeOfDay - last_shot > interval && inRange)
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
