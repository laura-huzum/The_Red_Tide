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
        if (!gamemanager.GetComponent<GameManagerBehavior>().stage_fight)
            updateGameData();

        // now call SAVE to save this Game in the file
        SaveLoad.Save();

        //SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene(0);
    }


    public void updateGameData()
    {
        List<GameObject> weplist = new List<GameObject>(GameObject.FindGameObjectsWithTag("DefensiveStructure"));
        GenericWeapon tmp;
        GameManagerBehavior gmb = gamemanager.GetComponent<GameManagerBehavior>();

        foreach (GameObject wpn in weplist)
        {
            tmp = wpn.GetComponent<GenericWeapon>();

            if (wpn.name.Contains("bigTower"))
            {
                //Debug.Log(wpn.transform.position);
                Debug.Log(Game.current.bigtowers);
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


        Game.current.reicshmark_count = gmb.reichsmark_stage_start;
        Game.current.stage_number = gmb.Stage;
        Game.current.level = 1;
    }


    public void continueEndless()
    {
        gamemanager.GetComponent<GameManagerBehavior>().continue_endless = true;
        gameObject.SetActive(false);
    }

    public void Retry()
    {

        // reload. data was saved right after a wave was defeated
        if (gamemanager.GetComponent<GameManagerBehavior>().Stage == 2) 
        {
            Game.current = null;
            
        }
        SceneManager.LoadScene("Level1");
    }
}
