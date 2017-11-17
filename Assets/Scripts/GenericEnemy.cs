using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GenericEnemy : MonoBehaviour
{

    public float speed = 1.0f;
    public DateTime spawnTime;
    public int hitpoints;
    public int bounty;
    public int damage;
    [HideInInspector]
    public GameObject gm;
    [HideInInspector]
    public GameManagerBehavior gmb;


}
