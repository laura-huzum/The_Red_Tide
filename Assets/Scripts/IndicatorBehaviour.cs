using UnityEngine;
using System.Collections;

public class IndicatorBehaviour : MonoBehaviour
{

    public GameObject bomber_prefab;
    GameObject bomber_instance;


    // Use this for initialization
    void Start()
    {
        Debug.Log("Here, object was instantiated");
        //gameObject.transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
    }

    // Update is called once per frame
    void Update()
    {
        
        gameObject.transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                gameObject.transform.position.y,
                                                -8);

    }

    private void OnMouseUp()
    {
        
        // instantiate bomber 
        // collider fixed in an area
        BoxCollider2D area_of_effect = gameObject.GetComponent<BoxCollider2D>();
        bomber_instance = Instantiate(bomber_prefab) as GameObject;
        //bomber_instance.AddComponent<Rigidbody2D>();
        bomber_instance.AddComponent<BoxCollider2D>();
        BoxCollider2D dmg_area = bomber_instance.GetComponent<BoxCollider2D>();
        dmg_area.enabled = false;
        dmg_area.transform.position = area_of_effect.transform.position;
        dmg_area.size = new Vector2(area_of_effect.size.x * area_of_effect.transform.localScale.x,
                                    area_of_effect.size.y * area_of_effect.transform.localScale.y);
        //dmg_area.transform.localScale = area_of_effect.transform.localScale;
        //dmg_area.isTrigger = true;

        bomber_instance.GetComponent<BomberBehaviour>().damage_area_collider = dmg_area;
        

        // destroy the gameObject
        Destroy(gameObject);
    }
}
