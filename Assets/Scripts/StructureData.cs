using UnityEngine;
using System.Collections;


//TODO: save the update level of the machine gun
[System.Serializable]
public class StructureData
{
    public int hitpoints;
    public float[] position;
    public int upgrade_level;

    public StructureData()
    {
        hitpoints = 0;
        position = new float[3];
        upgrade_level = 0;
    }

    public StructureData(int hp, Vector3 pos, int upgrade_level)
    {
        hitpoints = hp;
        position = new float[3];
        position[0] = pos.x;
        position[1] = pos.y;
        position[2] = pos.z;
        this.upgrade_level = upgrade_level;
    }


}
