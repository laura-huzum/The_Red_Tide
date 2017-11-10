using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour {



    // TODO: advance to next wave/level: update wave count and so on
    // TODO: objective lost and game lost

	public Text currencyLabel;
    public Text WaveCountLabel;
    public GameObject GameOverLabel;
    public bool gameOver = false;

    int special_cost;
    public GameObject special_indicator_prefab;

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
                // TODO: display GAME OVER
                // TODO: replace nazi flag with commie flag
                //GameObject gameOverText = GameObject.FindGameObjectWithTag("GameOver");
                //gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
            }
            // 4 

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
	}
	
	// Update is called once per frame
	void Update () {

        /*if (gameOver)
        {
            GameOverLabel.SetActive(false);
            // stop all activity
            // place russian flag
            // GAME OVER LABEL
        }*/

	
	}


    public void buySpecial()
    {
        if (Reichsmark > special_cost)
        {
            // spawn a semitransparent rectangle (the AoEIndicator) that follows the cursor
            // to indicate the area of effect.

            Instantiate(special_indicator_prefab);
        }
    }


}
