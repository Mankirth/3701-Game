using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;




public class GameManager : MonoBehaviour 
{
    [SerializeField]
    private int baseScore;

    [HideInInspector]
    public float score;

    [SerializeField]
    private TMP_Text scoreText, winScore, loseScore, multiplierText;

    private bool isPlaying;

    public int maxScore;

    [SerializeField]
    private Image grade;

    public Sprite gradeS, gradeA, gradeB, gradeC, gradeD;

    public int notorietyVal, RPVal;

    public Image promptImage;

    public Sprite good, perfect, dodge, miss;

    public Animator popupAnim;

    [SerializeField]
    private PlayerInput player;

    [Header("Point Totals")]
    [SerializeField]
    private int perfectPoints = 500;
    [SerializeField]
    private int goodPoints = 310;

    

    [Header("Gameplay Settings")]
    public PlayerSettings playerSettings;
    public TMP_Text currentDifficulty;
    public GameObject ButtonIcons;

    public static event Action OnPerfectParry;

    private Stack<int> perfectCount = new Stack<int>();
    private int pointMultiplier = 0;
    public void Start()
    {
        score = baseScore;
        isPlaying = true;
        //currentDifficulty.text = "Current Difficulty: " + playerSettings.difficulty.ToString();
    }

    // Make score increase on beat
    public void Update()
    {
        // Added this so you can change difficulty globally (i.e., outside of fights)
        // WARNING: TESTING NEEDED (there may be instances where points need to be added
        // but return too early)
        if (playerSettings == null || ButtonIcons == null)
        {
            return;
        }
        
        if (isPlaying)
        {
            score += 4 * Time.deltaTime;
            scoreText.text = "Score: " + Mathf.Round(score);
            if (score <= 0) score = 0;
        }

        if (playerSettings.inputIcon == PlayerSettings.InputIcon.Hide)
        {
            ButtonIcons.SetActive(false);
        }
        else
        {
            ButtonIcons.SetActive(true);
        }

    }

    public void ChangeDifficulty(int difficulty)
    {
        playerSettings.SetDifficultyPreset((PlayerSettings.Difficulty)difficulty); // Change how this works after. For now, 0 = Easy, 1 = Normal, 2 = Hard
        currentDifficulty.text = "Current Difficulty: " + playerSettings.difficulty.ToString();
    }



    public void AddParryScore()
    {
        
        
        if (player.inputTiming > 0.8)
        {
            IncreaseMultiplier();

            promptImage.sprite = perfect;
            score += perfectPoints * pointMultiplier;
            OnPerfectParry?.Invoke();
            player.gameObject.GetComponent<Health>().Heal();
            

        }
        else
        {
            if(playerSettings.healOnGood == PlayerSettings.HealOnGood.Enabled)
                player.gameObject.GetComponent<Health>().Heal();
            promptImage.sprite = good;
            score += goodPoints;

            ResetMultiplier();

        }
        player.inputTiming = 0;

        popupAnim.Play("FeedbackPrompt", 0, 0f);
    }

    public void DeductScore(int val, string type)
    {
        ResetMultiplier();
        score -= val;
        if (type == "dodge")
        {
            promptImage.sprite = dodge;
        }
        else
        {
            promptImage.sprite = miss;
        }
            
        popupAnim.Play("FeedbackPrompt", 0, 0f);
    }

    //public void DeductTimingScore()
    //{
    //    score -= 15;
    //    promptImage.sprite = dodge;
    //    popupAnim.Play("FeedbackPrompt", 0, 0f);
    //}

    public void CalculateFinalScore(int dodges)
    {
        isPlaying = false;
        score += dodges * 5;
        if (score <= 0) score = 0;
        winScore.text = "Final Score: " + Mathf.Round(score);
        loseScore.text = "Final Score: " + Mathf.Round(score);
        

        if (score >= maxScore * .95f) { 
            grade.sprite = gradeS;
        }
        if (score >= maxScore * .85f && score < maxScore * .95f) { 
            grade.sprite = gradeA;
    }
        if (score >= maxScore * .75f && score < maxScore * .85f) { 
            grade.sprite = gradeB;
        }
        if (score >= maxScore * .65f && score < maxScore * .75f) { 
            grade.sprite = gradeC;
        }
        if (score < maxScore * .65f) { 
            grade.sprite = gradeD;
        }
    }

    private void IncreaseMultiplier()
    {
        perfectCount.Push(1);
        pointMultiplier = Mathf.Clamp(1 + perfectCount.Count, 0, 10);

        multiplierText.text = "x" + pointMultiplier.ToString();
    }

    private void ResetMultiplier()
    {
        perfectCount.Clear();
        pointMultiplier = 1;
        multiplierText.text = "x1";
    }

}