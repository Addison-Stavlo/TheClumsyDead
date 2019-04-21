using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float score;
    float baseScore = 1000f;

    float waveTimer;

    Text scoreBoard;
    // Start is called before the first frame update
    void Start()
    {
        scoreBoard = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPoints()
    {
        waveTimer = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().waveTimer;
        float pointsToAdd = baseScore / waveTimer;
        score += pointsToAdd;
        scoreBoard.text = "SCORE: " + Mathf.RoundToInt(score);
    }
}
