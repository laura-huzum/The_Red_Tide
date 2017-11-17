using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManagerBehavior : MonoBehaviour {



    // TODO: advance to next wave/level: update wave count and so on
    // TODO: get a list of all enemies when they have finished spawning 
    // (meaning when the spawn script finished doing its job);
    // if they are all dead, then GAMEWON -> advance to next level or begin endless

    public Text currencyLabel;
    public Text StageCountLabel;

    public GameObject GameOverLabel;

    public int machineGun_cost;
    public int special_cost;
    public int trap_cost;
    public int tower_cost;
    public int time_between_waves;
    public GameObject buyMG_button;
    public GameObject buySpecial_button;
    public GameObject buyTower_button;
    public GameObject buyTrap_button;
    public GameObject special_indicator_prefab;
    public GameObject machinegun_prefab;
    public GameObject tower_prefab;
    public GameObject trap_prefab;
    public GameObject wall_prefab;
    public GameObject quitPrompt;
    public GameObject continueEndlessPrompt;
    public GameObject RetryPrompt;
    public GameObject Road;

    // used to determine if we place structures in this state or not
    [HideInInspector]
    public bool stage_fight = false;
    [HideInInspector]
    public bool changed_colors = false;
    [HideInInspector]
    public bool gameOver = false;
    [HideInInspector]
    public int stage_threshold = 3; // max 3 stages that define the threshold for passing onto the next level
    [HideInInspector]
    public bool spawn_finished = false;
    [HideInInspector]
    public bool continue_endless = false;
    [HideInInspector]
    public List<GameObject> living_enemies;
    [HideInInspector]
    public int reichsmark_stage_start;

    private SpawnStage stageSpawner;
    private bool prompted;
    

    //========================= SETS =============================

    private int stage;
    public int Stage
    {
        get { return stage; }
        set
        {
            stage = value;

            // TODO: display "START stage" message

            int smaller_stage = stage - 1;

            if (smaller_stage == 0)
            {
                StageCountLabel.text = "STAGE   1     [☆]";
            }
            else
            {
                StageCountLabel.text = "STAGE   " + smaller_stage + "     [";
                for (int i = 0; i < Mathf.Min(smaller_stage, 3); i++)
                {
                    StageCountLabel.text += "☆";
                }
                if (smaller_stage > 3)
                {
                    // stage 3+ --> endless
                    StageCountLabel.text += "+";
                }
                StageCountLabel.text += "]";
            }
        }
    }

    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            // 1
            if (value < health)
            {
                // if currently assigned health is greater than the update (aka damage taken)
                // APPLIABLE ONLY IF OBJECTIVE HAS MORE THAN 1 HITPOINT
                // like some sort of damage-taking effect
            }
            // 2
            health = value;
            // 3
            if (health <= 0 && !gameOver)
            {
                // TODO: add defeat prompt = restart stage or quit
                gameOver = true;
                GameOverLabel.SetActive(true);
                RetryPrompt.SetActive(true);

                // play defeat song
                /*AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.Play();*/
                // TODO: replace nazi flag with commie flag
            }


        }
    }

    private int reichsmark;
	public int Reichsmark {
  		get { return reichsmark; }
  		set
        {
            reichsmark = value;
            currencyLabel.GetComponent<Text>().text = reichsmark + "RM";
        }
    }



    //====================== FUNCTIONALITY ================================



    // Use this for initialization
    void Start () {
		Reichsmark = 800;
        Stage = 1;
        // for this level
        Health = 1;

        GameOverLabel.SetActive(false);
        Instantiate(wall_prefab).name = "Wall";
        stageSpawner = Road.GetComponent<SpawnStage>();
        living_enemies = new List<GameObject>();


        if (Game.current == null)
        {
            Game.current = new Game();
            //Debug.Log("new Game");
        }
        else
        {
            // LOAD GAME DATA
            // stage, money, structures
            loadSavedGameData();
        }
	}
	


	// Update is called once per frame
	void Update () {

        if (spawn_finished)
        {
            // check if there are any enemies left on the map
            for(int i = 0; i < living_enemies.Count; i++)
            {
                if (living_enemies[i] == null)
                    living_enemies.RemoveAt(i);
            }

            if (living_enemies.Count == 0 && stage_fight == true)
            {
                Debug.Log("all enemies are dead");
                stage_fight = false;
            }

        }

        if (stage_fight == false && stage == 4 && continue_endless == false && prompted == false)
        {
            // activate prompt
            continueEndlessPrompt.SetActive(true);
            prompted = true;

        }

        if (stage_fight && !changed_colors)
        {
            // disable buttons (machine gun, tower and start)
            // redden them
            // activate circle colliders
            Button[] btns = GameObject.FindObjectsOfType<Button>();
            foreach (Button btn in btns)
            {
                if (btn.name.Equals("startSpawnButton") ||
                    btn.name.Equals("BuyMachineGunButton") ||
                    btn.name.Equals("BuyTowerButton") ||
                    btn.name.Equals("BuyTrapButton"))
                {
                    btn.interactable = false;
                    btn.image.color = Color.red;
                }
            }
            changed_colors = true;

        }

        else if (!stage_fight && changed_colors)
        {
            Button[] btns = GameObject.FindObjectsOfType<Button>();
            // enable buttons (machine gun, tower and start)
            // whiten
            foreach (Button btn in btns)
            {
                if (btn.name.Equals("BuyMachineGunButton") ||
                    btn.name.Equals("BuyTowerButton") ||
                    btn.name.Equals("BuyTrapButton"))
                {
                    btn.interactable = true;
                    btn.image.color = Color.white;
                }
                if (btn.name.Equals("startSpawnButton"))
                {
                    btn.interactable = true;
                    btn.image.color = new Color(159f/255f,159f/255f,159f/255f, 255f/255f);
                }
            }
            changed_colors = false;
        }

	
	}


    public void buySpecial()
    {
        if (Reichsmark >= special_cost)
        {
            // spawn a semitransparent rectangle (the AoEIndicator) that follows the cursor
            // to indicate the area of effect.
            Instantiate(special_indicator_prefab);
            // deduct gold
            Reichsmark -= special_cost;
            
        }
        else
        {
            // flash red, can't afford
            reddenSpecial();
            Invoke("whitenSpecial", 0.25f);
        }
    }

    public void buyTrap()
    {
        if (!stage_fight)
        {
            if (Reichsmark >= trap_cost)
            {
                Instantiate(trap_prefab);
                // deduct gold
                Reichsmark -= trap_cost;
            }
            else
            {
                reddenTrap();
                Invoke("whitenTrap", 0.25f);
            }
        }


    }

    public void buyTower()
    {
        if (!stage_fight) {
            if (Reichsmark >= tower_cost)
            {
                Instantiate(tower_prefab);
                // deduct gold
                Reichsmark -= trap_cost;
            }
            else
            {
                //flash red
                reddenTower();
                Invoke("whitenTower", 0.25f);
            }
        }
    }

    public void buyMachineGun()
    {
        if (!stage_fight)
        {
            if (Reichsmark >= machineGun_cost)
            {
                Instantiate(machinegun_prefab);
                // deduct gold
                Reichsmark -= machineGun_cost;
            }
            else
            {
                //flash red
                reddenMG();
                Invoke("whitenMG", 0.25f);
            }
        }
    }


    public void promptQuit()
    {
        quitPrompt.SetActive(true);
    }



    //SET STAGE STRUCTURE ON BUTTON PRESS
    public void startStage()
    {
        // WAVE STRUCTURE parameters: wave_index, infantry_count, demoman_count, tank_count, time_to_next_wave (float)
        stage_fight = true;
        spawn_finished = false;
        reichsmark_stage_start = Reichsmark;
        stageSpawner.period_between_units = 1f;
        if (Stage == 1)
        {
           
            // set stage parameters
            stageSpawner.setWaveCount(4);
            stageSpawner.setWaveComposition(1, 3, 0, 0, time_between_waves);
            stageSpawner.setWaveComposition(2, 3, 0, 0, time_between_waves);
            stageSpawner.setWaveComposition(3, 2, 1, 0, time_between_waves);
            stageSpawner.setWaveComposition(4, 4, 1, 0, time_between_waves);

            // begin stage
            stageSpawner.startSpawn();
            Stage++;
        
        }

        else if (Stage == 2)
        {
            // set stage parameters
            stageSpawner.setWaveCount(6);
            stageSpawner.setWaveComposition(1, 3, 2, 0, time_between_waves);
            stageSpawner.setWaveComposition(2, 3, 0, 0, time_between_waves);
            stageSpawner.setWaveComposition(3, 0, 0, 1, time_between_waves*2);
            // GREATER INTERVAL BETWEEN THESE ^V TWO WAVES
            stageSpawner.setWaveComposition(4, 4, 1, 0, time_between_waves);
            stageSpawner.setWaveComposition(5, 5, 0, 0, time_between_waves);
            stageSpawner.setWaveComposition(6, 7, 2, 0, time_between_waves);

            // begin stage
            stageSpawner.startSpawn();
            Stage++;
        }

        else if (Stage == 3)
        {
            // set stage parameters
            stageSpawner.setWaveCount(7);
            stageSpawner.setWaveComposition(1, 5, 0, 0, time_between_waves);
            stageSpawner.setWaveComposition(2, 0, 4, 0, time_between_waves);
            stageSpawner.setWaveComposition(3, 2, 0, 2, time_between_waves);
            stageSpawner.setWaveComposition(4, 5, 0, 0, time_between_waves);
            stageSpawner.setWaveComposition(5, 5, 1, 1, time_between_waves*2);
            // GREATER INTERVAL BETWEEN THESE ^V TWO WAVES
            stageSpawner.setWaveComposition(6, 0, 2, 0, time_between_waves);
            stageSpawner.setWaveComposition(7, 4, 0, 2, time_between_waves);

            // begin stage
            stageSpawner.startSpawn();
            Stage++;
        }

        else
        {

            if (continue_endless)
            {
                int nr_infantry;
                int nr_demomen;
                int nr_tanks;
                int wave_size;
                
                // spawn a stage with a random composition and a random number of waves
                // starting at 7 waves per stage, increases to a maximum of 12 waves per stage
                // no more than ten beings in a wave
                // no more than ten infantry in a wave
                // no more than 7 demomen in a wave
                // no more than 5 tanks in a wave
                // bounty for all should be increased by 20% (new bounty = 1.2*current_bounty)
                Debug.Log("ENDLESS");
                stageSpawner.setWaveCount(stage + 7);
                
                for(int i = 1; i <= stageSpawner.NrWaves; i++)
                {
                    wave_size = Random.Range(8, 20);
                    nr_infantry = Random.Range(0, 6);
                    nr_demomen = Random.Range(0, wave_size - nr_infantry - 1);
                    nr_tanks = 1 + Random.Range(0, wave_size - nr_infantry - nr_demomen - 5);
                    stageSpawner.setWaveComposition(i+i, nr_infantry, nr_demomen, nr_tanks, time_between_waves);
                }




                stageSpawner.startSpawn();
                if (Stage < 9)
                    Stage++;
            }
        }

    }



    public void loadSavedGameData()
    {

        this.Stage = Game.current.stage_number;
        this.Reichsmark = Game.current.reicshmark_count;
        GameObject tmp;
        GenericWeapon wep;
        foreach (StructureData strucdata in Game.current.machineguns)
        {
            tmp = Instantiate(machinegun_prefab);
            wep = tmp.GetComponent<MachineGunBehaviour>();
            wep.shooting_state = true;
            tmp.layer = LayerMask.NameToLayer("Default");
            WeaponData wd = tmp.GetComponent<WeaponData>();
            wd.CurrentLevel = wd.levels[strucdata.upgrade_level];
            tmp.transform.position = new Vector3(strucdata.position[0], strucdata.position[1], strucdata.position[2]);
            wep.hitpoints = strucdata.hitpoints;

        }

        foreach (StructureData strucdata in Game.current.bigtowers)
        {
            tmp = Instantiate(tower_prefab);
            wep = tmp.GetComponent<BigTowerBehaviour>();
            wep.shooting_state = true;
            tmp.layer = LayerMask.NameToLayer("Default");

            tmp.transform.position = new Vector3(strucdata.position[0], strucdata.position[1], strucdata.position[2]);
            wep.hitpoints = strucdata.hitpoints;

        }

        foreach (StructureData strucdata in Game.current.traps)
        {
            tmp = Instantiate(trap_prefab);
            wep = tmp.GetComponent<TrapBehaviour>();
            wep.shooting_state = true;
            tmp.layer = LayerMask.NameToLayer("Default");

            tmp.transform.position = new Vector3(strucdata.position[0], strucdata.position[1], strucdata.position[2]);
            wep.hitpoints = strucdata.hitpoints;

        }

    }

    private void whitenSpecial()
    {
        buySpecial_button.GetComponent<Image>().color = Color.white;
    }

    private void reddenSpecial ()
    {
        buySpecial_button.GetComponent<Image>().color = Color.red;
    }



    private void whitenTower()
    {
        buyTower_button.GetComponent<Image>().color = Color.white;
    }

    private void reddenTower()
    {
        buyTower_button.GetComponent<Image>().color = Color.red;
    }

    private void whitenTrap()
    {
        buyTrap_button.GetComponent<Image>().color = Color.white;
    }

    private void reddenTrap()
    {
        buyTrap_button.GetComponent<Image>().color = Color.red;
    }

    private void whitenMG()
    {
        buyMG_button.GetComponent<Image>().color = Color.white;
    }

    private void reddenMG()
    {
        buyMG_button.GetComponent<Image>().color = Color.red;
    }

}
