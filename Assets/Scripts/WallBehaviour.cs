using UnityEngine;
using System.Collections;

public class WallBehaviour : MonoBehaviour
{

    public int hitpoints;

    // Update is called once per frame
    void Update()
    {
        if (hitpoints <= 0)
            Destroy(gameObject);
    }


    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("wall detecting collision");
    }*/
}
