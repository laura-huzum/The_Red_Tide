using UnityEngine;
using System.Collections;

public class TrapBehaviour : GenericWeapon
{

    bool onRoad;
    bool onTrap;

    // Use this for initialization
    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("HoverOver");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!shooting_state)
        {
            Debug.Log("Trap being placed but why tho");
            gameObject.transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                gameObject.transform.position.y,
                                                -0.1f);
        }
        if (hitpoints <= 0)
            Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision detected");
        if (collision.collider.gameObject.CompareTag("Road"))
        {
            onRoad = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if(collision.collider.gameObject.CompareTag("Trap")) {
            onTrap = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (shooting_state && collision.collider.gameObject.CompareTag("EnemyInfantry"))
        {
            //Debug.Log(collision.gameObject);

            gameObject.GetComponent<AudioSource>().Play();
            collision.collider.gameObject.GetComponent<MoveEnemy>().hitpoints -= damage;
            hitpoints -= 1;
            // change color for 0.1s
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            Invoke("whiten", 0.5f);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Road"))
        {
            onRoad = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (collision.collider.gameObject.CompareTag("Trap"))
        {
            onTrap = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }


    private void OnMouseUp()
    {
        //Debug.Log("click");
        Debug.Log("onRoad = " + onRoad + " onTrap = " + onTrap);
        if (!shooting_state && onRoad && !onTrap)
        {
            shooting_state = true;
            gameObject.layer = LayerMask.NameToLayer("Default");
            
        }
    }

    private void whiten()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

}
