using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MoveEnemy : GenericEnemy {

	[HideInInspector]
	public GameObject[] waypoints;
	private int currentWaypoint = 0;
	private float lastWaypointSwitchTime;
	
    // Use this for initialization
    void Start () {
		lastWaypointSwitchTime = Time.time;
        spawnTime = new DateTime();
        spawnTime = DateTime.Now;
        gm = GameObject.Find("GameManager");
        gmb = gm.GetComponent<GameManagerBehavior>();
        //bounty = 25;
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
    void Update () {
        // 1 

        if (!gmb.gameOver)
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
                       
                        gmb.Health -= 1;

                    }
                }
            }
            else
            {
                // add gold

                gmb.Reichsmark += bounty;
                Destroy(gameObject);  
            }
        }
	}



    private void OnCollisionEnter2D(Collision2D collision)
    {
        // collision with either trap, or bombardment
        //Debug.Log("collision with bombardment");
        BomberBehaviour bb = collision.gameObject.GetComponent<BomberBehaviour>();

        if (bb == null)
        {
            // trap behaviour

        }
        else
        {
            hitpoints -= bb.bombardment_damage;
        }


    }


}
