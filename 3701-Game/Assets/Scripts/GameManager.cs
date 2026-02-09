using System;
using TMPro;
using UnityEngine;



public class GameManager : MonoBehaviour 
{
    [SerializeField]
    private int baseScore;

    [HideInInspector]
    public float score;

    [SerializeField]
    private TMP_Text scoreText, winScore, loseScore;

    private bool isPlaying;


    public void Start()
    {
        score = baseScore;
        isPlaying = true;
    }

    // Make score increase on beat
    public void Update()
    {
        if (isPlaying)
        {
            score += 4 * Time.deltaTime;
            scoreText.text = "Score: " + Mathf.Round(score);
            Debug.Log("Player Score: " + score);
        }

    }

    // Get scores on phases


    public void AddParryScore()
    {
        score += 500;
    }

    public void DeductFailScore()
    {
        score -= 300;
    }

    public void CalculateFinalScore(int dodges)
    {
        isPlaying = false;
        score += dodges * 5;
        winScore.text = "Final Score: " + Mathf.Round(score);
        loseScore.text = "Final Score: " + Mathf.Round(score);
        Debug.Log("FINAL SCORE: " + score);
    }

}