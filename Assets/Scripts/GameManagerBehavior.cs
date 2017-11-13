using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour {



    // TODO: advance to next wave/level: update wave count and so on
    // TODO: get a list of all enemies when they have finished spawning 
    // (meaning when the spawn script finished doing its job);
    // if they are all dead, then GAMEWON -> advance to next level or begin endless

	public Text currencyLabel;
    public Text WaveCountLabel;
    public GameObject GameOverLabel;
    public bool gameOver = false;
    // used to determine if we place structures in this state or not
    public bool wave_fight = false;
    public bool changed_colors = false;

    int special_cost;
    int trap_cost;
    int tower_cost;
    public GameObject special_indicator_prefab;
    public GameObject trap_prefab;
    public GameObject tower_prefab;
    public GameObject wall_prefab;

    private int wave;
    public int Wave
    {
        get { return wave; }
        set
        {
            wave = value;
            
            // TODO: display "START WAVE" message

        
            WaveCountLabel.text = "WAVE: " + (wave + 1);
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
            }
            // 2
            health = value;
            //healthLabel.text = "HEALTH: " + health;
            // 3
            if (health <= 0 && !gameOver)
            {
                gameOver = true;
                GameOverLabel.SetActive(true);
                // TODO: replace nazi flag with commie flag
                //GameObject gameOverText = GameObject.FindGameObjectWithTag("GameOver");
                //gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
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

	// Use this for initialization
	void Start () {
		Reichsmark = 750;
        Wave = 0;
        // for this level
        Health = 1;

        // TODO: change this, we ain't bombin no one for free
        special_cost = 0;
        GameOverLabel.SetActive(false);
        Instantiate(wall_prefab).name = "Wall";
	}
	
	// Update is called once per frame
	void Update () {

        
        if (wave_fight && !changed_colors)
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
        else if (!wave_fight && changed_colors)
        {
            Button[] btns = GameObject.FindObjectsOfType<Button>();
            // enable buttons (machine gun, tower and start)
            // whiten
            foreach (Button btn in btns)
            {
                if (btn.name.Equals("startSpawnButton") ||
                    btn.name.Equals("BuyMachineGunButton") ||
                    btn.name.Equals("BuyTowerButton") ||
                    btn.name.Equals("BuyTrapButton"))
                {
                    btn.interactable = true;
                    btn.image.color = Color.white;
                }
            }
            changed_colors = false;
        }

	
	}


    public void buySpecial()
    {
        if (Reichsmark > special_cost)
        {
            // spawn a semitransparent rectangle (the AoEIndicator) that follows the cursor
            // to indicate the area of effect.

            Instantiate(special_indicator_prefab);
            
        }
        else
        {
            // flash red
        }
    }

    public void buyTrap()
    {
        if (!wave_fight)
        {
            if (Reichsmark > trap_cost)
            {
                Instantiate(trap_prefab);
            }
            else
            {
                //flash red
            }
        }
        else
        {
            // redden button
        }

    }

    public void buyTower()
    {
        if (!wave_fight) {
            if (Reichsmark > tower_cost)
            {
                Instantiate(tower_prefab);
            }
            else
            {
                //flash red
            }
        }
        else
        {
            // redden button
        }
    }

}
