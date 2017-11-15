using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class QuitSavePrompt : MonoBehaviour
{
    public GameObject gamemanager;

    public void QuitWithoutSaving()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelQuit()
    {
        gameObject.SetActive(false);
    }

    public void SaveQuit()
    {
        // TODO: add wall data LUL
        // build the currentGame instance and then call Save()
        List<GameObject> weplist = new List<GameObject>(GameObject.FindGameObjectsWithTag("DefensiveStructure"));
        GenericWeapon tmp;
        GameManagerBehavior gmb = gamemanager.GetComponent<GameManagerBehavior>();

        foreach (GameObject wpn in weplist)
        {
            tmp = wpn.GetComponent<GenericWeapon>();

            if (wpn.name.Contains("bigTower"))
            {
                //Debug.Log(wpn.transform.position);
                Game.current.bigtowers.Add(new StructureData(tmp.hitpoints, wpn.transform.position, 0));
            }
            else if (wpn.name.Contains("machineGun"))
            {
                Game.current.machineguns.Add(new StructureData(tmp.hitpoints, wpn.transform.position, wpn.gameObject.GetComponent<WeaponData>().curr_level));
            }
        }

        weplist = new List<GameObject>(GameObject.FindGameObjectsWithTag("Trap"));

        foreach (GameObject wpn in weplist)
        {
            tmp = wpn.GetComponent<GenericWeapon>();
            Game.current.traps.Add(new StructureData(tmp.hitpoints, wpn.transform.position, 0));
        }


        Game.current.reicshmark_count = gmb.Reichsmark;
        Game.current.wave_number = gmb.Wave;
        Game.current.level = 1;

        // now call SAVE to save this Game in the file
        SaveLoad.Save();

        //SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene(0);

        /*if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // save last wave
            // save amount of gold
            // save the structures
            // basically everything that wasn't an enemy
            GameManagerBehavior gm = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
            PlayerPrefs.SetInt("Reichsmark", gm.Reichsmark);
            PlayerPrefs.SetInt("Level", 1); // TODO: don't hardcode the current level :^)
            PlayerPrefs.SetInt("Wave", gm.Wave);
            // TODO: CROSSPLATFORM FILESYSTEM
            //string path = "C:\\users\\" + System.Environment.UserName + "\\Documents\\TheRedTide";
            //System.IO.Directory.CreateDirectory(@path);
            string path = System.IO.Directory.GetCurrentDirectory();
            //Debug.Log(path);
            System.IO.File.Create(path + "\\SGMData");
            System.IO.StreamWriter file =
                new System.IO.StreamWriter(path+ "\\SGMData");


            // SAVED GAME STRUCTURE:
            // BIGTOWERS DATA: a line with position and hitpoints for every instance
            // MACHINEGUNS DATA: -||-
            // TRAPS DATA:  -||-


            List<GameObject> weplist = new List<GameObject>(GameObject.FindGameObjectsWithTag("DefensiveStructures"));
            GenericWeapon tmp;
            file.WriteLine("====BIG TOWER DATA====");

            foreach (GameObject wpn in weplist)
            {
                if (wpn.name.Contains("bigTower"))
                {
                    tmp = wpn.GetComponent<GenericWeapon>();
                    file.WriteLine(wpn.transform.position + "," + tmp.hitpoints);
                }
            }


            file.WriteLine("====MACHINE GUN DATA====");

            foreach (GameObject wpn in weplist)
            {
                if (wpn.name.Contains("machineGun"))
                {
                    tmp = wpn.GetComponent<GenericWeapon>();
                    file.WriteLine(wpn.transform.position + "," + tmp.hitpoints);
                }
            }

            weplist = new List<GameObject>(GameObject.FindGameObjectsWithTag("Trap"));

            file.WriteLine("====TRAP DATA====");

            foreach (GameObject wpn in weplist)
            {
 
                tmp = wpn.GetComponent<GenericWeapon>();
                file.WriteLine(wpn.transform.position + "," + tmp.hitpoints);

            }

            SceneManager.LoadScene("MainMenu");
        }*/

    }
}
