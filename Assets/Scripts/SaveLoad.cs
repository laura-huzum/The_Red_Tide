using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 
public static class SaveLoad
{
    public static List<Game> savedGames = new List<Game>();


    // LOADS ALL SAVED GAMES
    // MUST BE CALLED ON THE MAIN MENU
    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.sgm"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.sgm", FileMode.Open);
            SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
    }



    public static void Save()
    {
        // 5 = NUMBER OF SAVED GAME SLOTS
        if (savedGames.Count == 5)
            savedGames.RemoveAt(0);
       
        savedGames.Add(Game.current);
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log(Application.persistentDataPath);
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.sgm");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
      
    }



    
}
