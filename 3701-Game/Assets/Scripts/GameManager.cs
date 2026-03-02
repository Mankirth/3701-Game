using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour 
{
    [SerializeField]
    private int baseScore;

    [HideInInspector]
    public float score;

    [SerializeField]
    private TMP_Text scoreText, winScore, loseScore;

    private bool isPlaying;

    public int maxScore;

    [SerializeField]
    private Image grade;

    public Sprite gradeS, gradeA, gradeB, gradeC, gradeD;

    public int notorietyVal, RPVal;

    public Image promptImage;

    public Sprite good, perfect, dodge;

    public Animator popupAnim;

    [SerializeField]
    private PlayerInput player;

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
            if (score <= 0) score = 0;
        }
        

    }



    public void AddParryScore()
    {
        score += 500;
        if (player.inputTiming > 0.8)
        {
            promptImage.sprite = perfect;
        }
        else
        {
            promptImage.sprite = good;
        }

            popupAnim.Play("FeedbackPrompt", 0, 0f);
    }

    public void DeductFailScore()
    {
        score -= 300;
        promptImage.sprite = dodge;
        popupAnim.Play("FeedbackPrompt", 0, 0f);
    }

    public void CalculateFinalScore(int dodges)
    {
        isPlaying = false;
        score += dodges * 5;
        if (score <= 0) score = 0;
        winScore.text = "Final Score: " + Mathf.Round(score);
        loseScore.text = "Final Score: " + Mathf.Round(score);
        

        if (score >= maxScore * .95f) { 
            grade.sprite = gradeS;
            Debug.Log("GRADE S");
        }
        if (score >= maxScore * .85f && score < maxScore * .95f) { 
            grade.sprite = gradeA;
        Debug.Log("GRADE A");
    }
        if (score >= maxScore * .75f && score < maxScore * .85f) { 
            grade.sprite = gradeB;
            Debug.Log("GRADE B");
        }
        if (score >= maxScore * .65f && score < maxScore * .75f) { 
            grade.sprite = gradeC;
            Debug.Log("GRADE C");
        }
        if (score < maxScore * .65f) { 
            grade.sprite = gradeD;
            Debug.Log("GRADE D");
        }
    }

}