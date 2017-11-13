using UnityEngine;
using System.Collections;
using System;
using Boo.Lang;

public class SpawnEnemy : MonoBehaviour {

	public GameObject[] waypoints;
	public GameObject Infantry;
    public GameObject Tank;
    public GameObject Demoman;
    public int NrWaves;
    public int EnemiesPerWave;
    public int DemomanPerWave;
    public int TanksPerWave;
    public float Period;
    public float TimeToNext;

    // Use this for initialization
    void Start () {
        Infantry.gameObject.tag = "EnemyInfantry";
        Tank.gameObject.tag = "EnemyTank";
    }

    void Russboi()
    {
        Instantiate(Infantry).GetComponent<MoveEnemy>().waypoints = waypoints;
    }

    void T34()
    {
        
        Instantiate(Tank).GetComponent<MoveTank>().waypoints = waypoints;
    }

    void Mandemo()
    {
        Instantiate(Demoman).GetComponent<MoveDemoman>().waypoints = waypoints;
    }

    public void startSpawn()
    {
        GameObject.Find("GameManager").GetComponent<GameManagerBehavior>().wave_fight = true;
        StartCoroutine(Waver());
    }

    public IEnumerator Waver()
    {
        for(int i=0; i<NrWaves;i++)
            yield return StartCoroutine(GenerateWave());
    }

    public IEnumerator GenerateWave()
    {
        int spawned = 0;
        int tanks = 0;
        int demo = 0;
        while (spawned < EnemiesPerWave)
        {
            spawned++;
            Russboi();
            yield return new WaitForSeconds(Period);
        }

        while (demo < DemomanPerWave)
        {
            demo++;
            Mandemo();
            yield return new WaitForSeconds(Period);
        }

        while (tanks < TanksPerWave)
        {
            tanks++;
            T34();
            yield return new WaitForSeconds(Period);
        }
        yield return new WaitForSeconds(TimeToNext);
    }

    // Update is called once per frame
    void Update () {

    }
}
