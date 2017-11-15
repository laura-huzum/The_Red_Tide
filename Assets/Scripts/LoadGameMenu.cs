using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGameMenu : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject LoadingImage;

    // Use this for initialization
    void Start()
    {
        // add saved games list to canvas
        // as buttons
        // the first element in the list is the oldest. 
        // but the saved slot we want to display should be the most recent
        // so the buttons will be instantiated from bottom to top
        GameObject[] gameSlotButtons = GameObject.FindGameObjectsWithTag("GameSlot");


        int i = 0;
        int j;
        int n = SaveLoad.savedGames.Count - 1;
        foreach (Game saveGame in SaveLoad.savedGames)
        {
            Debug.Log(n - i);
            for (j = 0; j < 5; j++)
            {
                if (gameSlotButtons[j].name.Contains((n - i).ToString()))
                    gameSlotButtons[j].transform.Find("Text").gameObject.GetComponent<Text>().text = "LEVEL " + saveGame.level + " - WAVE " + saveGame.wave_number;
            }
            i++;
        }
    }
    

    public void backToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // method to be assigned to each slot-button
    public void loadSaveGameSlot(int i)
    {
        // load the saved game in the savedGames[i]
        if (i < SaveLoad.savedGames.Count)
        {
            Game.current = SaveLoad.savedGames[i];
            SceneManager.LoadScene("Level" + Game.current.level);
        }

        LoadingImage.SetActive(true);
    }





}
