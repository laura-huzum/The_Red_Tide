using UnityEngine;
using System.Collections;

public class BomberBehaviour : MonoBehaviour
{
    public float bomber_speed;
    public int bombardment_damage;
    [HideInInspector]
    public BoxCollider2D damage_area_collider;
    private Vector3 spawn_position;
    private Transform bomber_sprite;
    private Vector3 distance;
    private float screen_width_world;
    



    // Use this for initialization
    void Start()
    {
        //gameObject.transform.
        // hide the scorches
        screen_width_world = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height)).x;

        foreach (Transform child in transform)
        {
            if (child.name.Contains("Scorch"))
                child.gameObject.SetActive(false);
        }
        bomber_sprite = transform.Find("bomberSprite");
        //Debug.Log(bomber_sprite.gameObject.active);
       

        spawn_position = damage_area_collider.transform.position;
        // spawn the bomber on the right side of the screen, out of bounds
        spawn_position.z = -2;
        spawn_position.x =  screen_width_world + bomber_sprite.GetComponent<SpriteRenderer>().bounds.size.x;
        // play sound effect
        //Debug.Log(bomber_sprite);
        //Debug.Log(damage_area_collider);
        gameObject.GetComponent<AudioSource>().Play();
        bomber_sprite.position = spawn_position;
        Debug.Log(damage_area_collider.transform.position);
        Debug.Log(bomber_sprite.position);

    }

    // Update is called once per frame
    void Update()
    {
        // move plane across the map, on the x axis
        if (bomber_sprite.position.x > -screen_width_world - bomber_sprite.GetComponent<SpriteRenderer>().bounds.size.x)
        {
            
            distance = new Vector3(bomber_speed * Time.deltaTime, 0, 0);
            bomber_sprite.position -= distance;

            // activate collider when plane hovers over it (its center?)
            // CAREFUL ON Z, 2D colliders don't have a Z component
            if (Mathf.Abs(bomber_sprite.position.x  - damage_area_collider.transform.position.x) < 0.05)
            {
                Debug.Log("hovering over collider");
                // activate collider so it can deal damage for like... dunno, 0.5s
                damage_area_collider.enabled = true;
                Invoke( "disable_collider" , 0.5f);

            }

        }
        else
        {
            // fadeout holes
            // destroy object after fadeout
            // Debug.Log("destroy object");
            Destroy(gameObject);

        }

    }


    private void disable_collider()
    {
        damage_area_collider.enabled = false;
    }

}
