using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBehaviour : MonoBehaviour
{
    
    public GameObject structure_prefab;
    GameObject structure;
    bool shooting_state;
    bool isColliding;

    // Use this for initialization
    void Start()
    {



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

        if(!shooting_state && !isColliding)
        {
            // object is being placed
            shooting_state = true;
        }


    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision entered");
        isColliding = true;

        // the transform member, although not initialized in the behavioural script,
        // is a member of the GameObject class, and it is tied to the GameObject to 
        // which the script is assigned.
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Collision exitted");
        isColliding = false;
        // turn back to default
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
