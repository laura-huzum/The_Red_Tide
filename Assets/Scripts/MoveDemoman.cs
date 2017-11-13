using UnityEngine;
using System;
using System.Collections;

public class MoveDemoman : GenericEnemy
{

    [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;


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
        Vector3 newEndPosition;
       
        if (newStartPosition == waypoints[10].transform.position && GameObject.Find("Wall") == null
            && gameObject.transform.position.y <= waypoints[10].transform.position.y)
        {
            newEndPosition = waypoints[currentWaypoint + 3].transform.position;
        }
        else
        {
            newEndPosition = waypoints[currentWaypoint + 1].transform.position;
        }
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
                Vector3 endPosition;

                if (startPosition == waypoints[10].transform.position && GameObject.Find("Wall") == null
                    && gameObject.transform.position.y <= waypoints[10].transform.position.y)
                {
                    endPosition = waypoints[currentWaypoint + 3].transform.position;
                }
                else
                {
                    endPosition = waypoints[currentWaypoint + 1].transform.position;
                }
                // 2 
                float pathLength = Vector3.Distance(startPosition, endPosition);
                float totalTimeForPath = pathLength / speed;
                float currentTimeOnPath = Time.time - lastWaypointSwitchTime;

                gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
                // 3 
                if (gameObject.transform.position.Equals(endPosition))
                {
                    if (currentWaypoint < waypoints.Length - 2)
                    {
                        // 4 Switch to next waypoint

                        if (startPosition == waypoints[10].transform.position && GameObject.Find("Wall") == null
                            && gameObject.transform.position.y <= waypoints[10].transform.position.y)
                            currentWaypoint += 3;
                        else currentWaypoint++;
                        lastWaypointSwitchTime = Time.time;
                        // rotate into move direction
                        RotateIntoMoveDirection();
                    }
                    else
                    {
                        // enemy reached final objective
                        Destroy(gameObject);

                        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

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
        // collision with either trap, or bombardment
        //Debug.Log("collision with bombardment");
        BomberBehaviour bb = collision.gameObject.GetComponent<BomberBehaviour>();
        Debug.Log("collision");
        if (bb == null)
        {

            // trap behaviour
            // collision with some structure
            if (collision.collider.GetType().Equals(typeof(BoxCollider2D)))
            {
                if (collision.gameObject.CompareTag("DefensiveStructure"))
                {
                    // deal dmg to the structure
                    BigTowerBehaviour bscript = collision.gameObject.GetComponent<BigTowerBehaviour>();
                    MachineGunBehaviour mgscript;
                    WallBehaviour wscript;
                    if (bscript == null)
                    {
                        mgscript = collision.gameObject.GetComponent<MachineGunBehaviour>();
                        if (mgscript == null)
                        {
                            Debug.Log("Wall detected");
                            wscript = collision.gameObject.GetComponent<WallBehaviour>();
                            wscript.hitpoints -= damage;
                        }
                        else
                        {
                            mgscript.hitpoints -= damage;
                        }
                    }
                    else
                    {
                        bscript.hitpoints -= damage;
                    }
                }
            }

        }
        else
        {
            hitpoints -= bb.bombardment_damage;
        }


    }
}
