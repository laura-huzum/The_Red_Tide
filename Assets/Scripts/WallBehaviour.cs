using UnityEngine;
using System.Collections;

public class WallBehaviour : GenericWeapon
{

    // Update is called once per frame
    void Update()
    {
        if (hitpoints <= 0)
            Destroy(gameObject);
    }
}
