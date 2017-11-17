using UnityEngine;
using System.Collections;
using System;
using Boo.Lang;

public class SpawnStage : MonoBehaviour {

	public GameObject[] waypoints;
	public GameObject Infantry;
    public GameObject Tank;
    public GameObject Demoman;
    public GameObject gm;

    private GameManagerBehavior gameManager;
    private GameObject enemy_instance;

    [HideInInspector]
    public int NrWaves;
    public float period_between_units;
    


    public class WaveComposition
    {
        public int InfantryPerWave;
        public int DemomenPerWave;
        public int TanksPerWave;
        public float time_to_next_wave;
    }

    public WaveComposition[] wavesStructure;



    // Use this for initialization
    void Start () {
        gameManager = gm.GetComponent<GameManagerBehavior>();
    }

    public void setWaveCount(int waveCount)
    {
        NrWaves = waveCount;
        wavesStructure = new WaveComposition[NrWaves];
        for (int i = 0; i < waveCount; i++)
            wavesStructure[i] = new WaveComposition();
    }

    public void setWaveComposition(int wave_indx, int infantryCount, int demoCount, int tankCount, float time_to_next_wave)
    {
        // wave_index from 1 to NrWaves, but indx from 0 to NrWaves-1
        int indx = wave_indx - 1;
        if (indx < NrWaves)
        {
           
            wavesStructure[indx].InfantryPerWave = infantryCount;
            wavesStructure[indx].DemomenPerWave = demoCount;
            wavesStructure[indx].TanksPerWave = tankCount;
            wavesStructure[indx].time_to_next_wave = time_to_next_wave;
        }
    }

    void Russboi()
    {
        enemy_instance = Instantiate(Infantry);
        enemy_instance.GetComponent<MoveEnemy>().waypoints = waypoints;
        gameManager.living_enemies.Add(enemy_instance);
    }

    void T34()
    {

        enemy_instance = Instantiate(Tank);
        enemy_instance.GetComponent<MoveTank>().waypoints = waypoints;
        gameManager.living_enemies.Add(enemy_instance);
    }

    void Demolitiondude()
    {
        enemy_instance = Instantiate(Demoman);
        enemy_instance.GetComponent<MoveDemoman>().waypoints = waypoints;
        gameManager.living_enemies.Add(enemy_instance);
    }



    

    // begin spawning wave number [-wave-]
    // useful if we need to modify the spawn interval between two particular waves
    public void startSpawn()
    {
        //WDebug.Log("Here1");
        StartCoroutine(Stager());
    }

    

    public IEnumerator Stager()
    {
        //Debug.Log("Here2");
        yield return StartCoroutine(GenerateStage());
    }


    public IEnumerator GenerateStage()
    {
        //Debug.Log("Here3");
        int wave_indx;
        int infantry;
        int tanks;
        int demo;
        WaitForSeconds period = new WaitForSeconds(period_between_units);

        for (wave_indx = 0; wave_indx < NrWaves; wave_indx++)
        {
            infantry = 0;
            tanks = 0;
            demo = 0;

            while (infantry < wavesStructure[wave_indx].InfantryPerWave)
            {
                infantry++;
                Russboi();
                yield return period;
            }


            while (demo < wavesStructure[wave_indx].DemomenPerWave)
            {
                demo++;
                Demolitiondude();
                yield return period;
            }


            while (tanks < wavesStructure[wave_indx].TanksPerWave)
            {
                tanks++;
                T34();
                yield return new WaitForSeconds(period_between_units * 5);
            }

            if (wave_indx < NrWaves - 1)
            {
                yield return new WaitForSeconds(wavesStructure[wave_indx].time_to_next_wave);
            }
        }

        Debug.Log("finished spawning");
        gameManager.spawn_finished = true;
        
      
    }

}
