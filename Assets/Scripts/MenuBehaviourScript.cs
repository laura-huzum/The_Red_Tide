﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviourScript : MonoBehaviour {
    
    public GameObject loadingImage;

    public void Start()
    {
        // load saved games
        SaveLoad.Load();
    }

    public void StartNewGame()
    {
        loadingImage.SetActive(true);
        Game.current = null;
        SceneManager.LoadScene("Level1");
        
    }

    public void LoadSavedGamesMenu()
    {
        SceneManager.LoadScene("LoadGames");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
