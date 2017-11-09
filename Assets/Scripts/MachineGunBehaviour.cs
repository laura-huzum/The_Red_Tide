using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBehaviour : MonoBehaviour
{
    
    public GameObject structure_prefab;
    GameObject structure;
    bool shooting_state;
    bool isColliding;
    private TimeSpan last_shot;
    private TimeSpan interval = new TimeSpan(0,0,1);
    private List<GameObject> enemyList;
    private double seconds_oldest_target;
    private GameObject target;


    // Use this for initialization
    void Start()
    {
        seconds_oldest_target = 0;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        enemyList = new List<GameObject>();


    }




    // Update is called once per frame
    void Update()
    {

        if (!shooting_state)
        {
            gameObject.transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
        }
        else
        {
            // shooting at stuff
            

            
        }

    }


    public void PlaceAction()
    {

        structure = (GameObject)Instantiate(structure_prefab, Camera.main.ScreenToViewportPoint(Input.mousePosition), Quaternion.identity);
        shooting_state = false;

    }

    void OnMouseDown()
    {

        if (!shooting_state && !isColliding)
        {
            // object is being placed
            shooting_state = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
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
            }
        else
        {
            if (collision.collider.gameObject.CompareTag("EnemyInfantry"))
            {
                enemyList.Add(collision.collider.gameObject);
                Debug.Log("Enemy list add " + enemyList.Count);
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
            Debug.Log("Enemy list remove " + enemyList.Count);
        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (shooting_state && enemyList.Count >= 1)
        {
            //get array of enemies
            //collision.otherCollider.transform.parent.gameObject;

            update_oldest_enemy();

            if (DateTime.Now.TimeOfDay - last_shot > interval)
            {

                Debug.Log("shooting at this old boy: " + seconds_oldest_target);
                last_shot = DateTime.Now.TimeOfDay;
                // remove HP
                MoveEnemy script = target.GetComponent<MoveEnemy>();
                // replace with damage
                script.hitpoints -= 1;


                Vector3 newStartPosition = gameObject.transform.position;
                Vector3 newEndPosition = target.transform.position;
                Vector3 newDirection = (newEndPosition - newStartPosition);
                //2
                float x = newDirection.x;
                float y = newDirection.y;
                float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
                gameObject.transform.Find("machine_gun_top_sprite").rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

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
                Debug.Log("Enemy list remove " + enemyList.Count);
                i--;
            }
            else
            {
                MoveEnemy script = enemyList[i].GetComponent<MoveEnemy>();

                if (script == null)
                    Debug.Log("fmm" + enemyList[i] + " instID: " + this.GetInstanceID());
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
