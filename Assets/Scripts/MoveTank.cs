using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MoveTank : GenericEnemy
{
    [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    private GameObject target;
    private TimeSpan last_shot;
    private TimeSpan interval = new TimeSpan(0, 0, 0, 0, 2000); // 2000 ms

    bool moves = true;

    // Use this for initialization
    void Start()
    {
        lastWaypointSwitchTime = Time.time;
        spawnTime = new DateTime();
        spawnTime = DateTime.Now;
        bounty = 25;
        //hitpoints = 2;
    }


    private void RotateIntoMoveDirection()
    {
        //1
        Vector3 newStartPosition = waypoints[currentWaypoint].transform.position;
        Vector3 newEndPosition = waypoints[currentWaypoint + 1].transform.position;
        Vector3 newDirection = (newEndPosition - newStartPosition);
        //2
        float x = newDirection.x;
        float y = newDirection.y;
        float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
        //3

        gameObject.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

    }

    // Update is called once per frame
    void Update()
    {
        // 1 

        if (!GameObject.Find("GameManager").GetComponent<GameManagerBehavior>().gameOver)
        {
            if (hitpoints > 0)
            {
                Vector3 startPosition = waypoints[currentWaypoint].transform.position;
                Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;
                // 2 
                float pathLength = Vector3.Distance(startPosition, endPosition);
                float totalTimeForPath = pathLength / speed;
                // TODO: it rushes if it spends too much time between the same waypoints
                float currentTimeOnPath = Time.time - lastWaypointSwitchTime;

                if (moves)
                {
                    gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
                }
                if (!moves)
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                // 3 
                if (gameObject.transform.position.Equals(endPosition))
                {
                    if (currentWaypoint < waypoints.Length - 2)
                    {
                        // 4 Switch to next waypoint
                        currentWaypoint++;
                        lastWaypointSwitchTime = Time.time;
                        // rotate into move direction
                        RotateIntoMoveDirection();
                    }
                    else
                    {
                        // enemy reached final objective
                        Destroy(gameObject);

                        //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                        //AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

                        // deduct health
                        GameManagerBehavior gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
                        gameManager.Health -= 1;

                    }
                }
            }
            else
            {
                // add gold

                GameObject.Find("GameManager").GetComponent<GameManagerBehavior>().Reichsmark += bounty;
                Destroy(gameObject);
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision with bombardment");
        BomberBehaviour bb = collision.gameObject.GetComponent<BomberBehaviour>();

        if (bb == null)
        {
            // trap behaviour
        }
        else
        {
            hitpoints -= bb.bombardment_damage;
        }
        if (collision.collider.gameObject.CompareTag("DefensiveStructure") && collision.collider.GetType().Equals(typeof(BoxCollider2D)))
        {
            moves = false;
            target = collision.collider.gameObject;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.collider.gameObject.GetType().Equals(typeof(GenericWeapon)))
        moves = true;
        //transform.Find("tank_top").rotation = Quaternion.AngleAxis(Mathf.Atan2(transform.position.y, transform.position.x) * 180 / Mathf.PI, Vector3.forward);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // if collided with a defensive structure that is not a trap
        if (collision.collider.gameObject.GetComponent<GenericWeapon>()!= null)
        {
            if (DateTime.Now.TimeOfDay - last_shot > interval )
            {
                Debug.Log("tank shooting at nothing" + collision.collider.gameObject.name);

                //Debug.Log("shooting at this old boy: " + seconds_oldest_target);
                // play sound
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                last_shot = DateTime.Now.TimeOfDay;

                // remove HP
                GenericWeapon script = target.GetComponent<GenericWeapon>();
                // replace with damage
                script.hitpoints -= damage;


                Vector3 newStartPosition = gameObject.transform.position;
                Vector3 newEndPosition = target.transform.position;
                Vector3 newDirection = (newEndPosition - newStartPosition);
                //2
                float x = newDirection.x;
                float y = newDirection.y;
                float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;

                transform.Find("tank_top").rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

            }
        }
    }
}
