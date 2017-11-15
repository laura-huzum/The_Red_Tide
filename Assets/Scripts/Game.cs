using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Game
{

    public static Game current;

    public List<StructureData> bigtowers;
    public List<StructureData> machineguns;
    public List<StructureData> traps;
    public int wave_number;
    public int level;
    public int reicshmark_count;

   public Game()
   {
        bigtowers = new List<StructureData>();
        machineguns = new List<StructureData>();
        traps = new List<StructureData>();
   }

}
