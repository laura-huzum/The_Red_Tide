using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBehaviour : MonoBehaviour
{

    GameObject structure_prefab;
    GameObject structure;
    bool shooting_state;

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

        if(!shooting_state)
        {
            // object is being placed
            shooting_state = true;
        }


    }
}
