using UnityEngine;
using System.Collections;

public class TrapBehaviour : MonoBehaviour
{

    public int trap_damage;
    public int uses;
    bool onRoad;
    bool onTrap;

    bool trapping_state;

    // Use this for initialization
    void Start()
    {
        trapping_state = false;
        gameObject.layer = LayerMask.NameToLayer("HoverOver");
    }

    // Update is called once per frame
    void Update()
    {

        if (!trapping_state)
        {
            gameObject.transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 1);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                gameObject.transform.position.y,
                                                -0.1f);
        }
        if (uses <= 0)
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
        else if (trapping_state && collision.collider.gameObject.CompareTag("EnemyInfantry"))
        {
            //Debug.Log(collision.gameObject);

            gameObject.GetComponent<AudioSource>().Play();
            collision.collider.gameObject.GetComponent<MoveEnemy>().hitpoints -= trap_damage;
            uses -= 1;
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
        if (!trapping_state && onRoad && !onTrap)
        {
            trapping_state = true;
            gameObject.layer = LayerMask.NameToLayer("GroundBound");
            /*Vector2 current_size = gameObject.GetComponent<BoxCollider2D>().size;
            Vector2 gameObject_scale = gameObject.transform.localScale;

            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(current_size.x * gameObject_scale.x * 0.05f,
                                                                        current_size.y * gameObject_scale.y * 0.05f);*/
        }
    }

    private void whiten()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

}
