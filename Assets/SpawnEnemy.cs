using UnityEngine;
using System.Collections;
using System;
using Boo.Lang;

public class SpawnEnemy : MonoBehaviour {

	public GameObject[] waypoints;
	public GameObject Infantry;
    private TimeSpan timestart;

    // Use this for initialization
    void Start () {
        timestart = DateTime.Now.TimeOfDay;
        Infantry.gameObject.tag = "EnemyInfantry";
        StartCoroutine(GenerateWave());
    }
    void Russboi()
    {
        Instantiate(Infantry).GetComponent<MoveEnemy>().waypoints = waypoints;
    }
    IEnumerator GenerateWave()
    {
        int spawned = 0;
        while (spawned < 5)
        {
            spawned++;
            Russboi();
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(5.0f);
        spawned = 0;
        while (spawned < 5)
        {
            spawned++;
            Russboi();
            yield return new WaitForSeconds(1.5f);
        }
    }

    // Update is called once per frame
    void Update () {

    }
}
