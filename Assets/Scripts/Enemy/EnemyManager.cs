using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

    public int startingEnemies = 5;
    public int morePerWave = 2;
    public int waveNumber;

    public float waveTimer;
    // Start is called before the first frame update
    GameObject[] liveEnemies;
    bool respawn = false;
    bool spawned = false;
    Text waveCounter;
    Text waveTimerText;
    void Start()
    {
        liveEnemies = new GameObject[0];
        waveCounter = GameObject.FindGameObjectWithTag("WaveCounter").GetComponent<Text>();
        waveTimerText = GameObject.FindGameObjectWithTag("WaveTimer").GetComponent<Text>();
        Spawn();

    }

    // Update is called once per frame
    void Update()
    {
        waveTimer += Time.deltaTime;
        waveTimerText.text = "Time in Wave: " + Mathf.RoundToInt(waveTimer) + " sec";
        if (spawned)
        {
            respawn = true;
            foreach (GameObject enemy in liveEnemies)
            {
                if (!enemy.GetComponent<SkeleController>().isDead)
                {
                    respawn = false;
                }
            }

            if (respawn)
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        if (liveEnemies.Length > 0)
        {
            foreach (GameObject enemy in liveEnemies)
            {
                Destroy(enemy, 2);
            }
        }
        waveNumber += 1;
        waveCounter.text = "Wave: " + waveNumber;
        liveEnemies = new GameObject[startingEnemies];
        for (int i = 0; i < startingEnemies; i++)
        {
            liveEnemies[i] = Instantiate(enemy, spawnPoints[i % spawnPoints.Length].position, spawnPoints[i % spawnPoints.Length].rotation);
        }
        spawned = true;
        waveTimer = 0f;
        startingEnemies += morePerWave;
        morePerWave *= 2;
    }
}
