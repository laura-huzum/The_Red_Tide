using UnityEngine;
using System.Collections;



// class to use to save the data of the prefabs
public class GenericWeapon : MonoBehaviour
{

    public int hitpoints;
    public int damage;
    public bool shooting_state;
    [HideInInspector]
    public GameObject gm;
    [HideInInspector]
    public GameManagerBehavior gmb;


}
