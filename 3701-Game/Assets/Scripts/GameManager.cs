using System;
using System.Collections.Generic;
using System.Threading;
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

    [SerializeField]
    private MusicManager musicManager;
    private float songLength;

    [Header("Point Totals")]
    [SerializeField]
    private int perfectPoints = 550;
    [SerializeField]
    private int goodPoints = 500;
    public int goodTotalCount { get; private set; }
    public int perfectTotalCount { get; private set; }
    public int missTotalCount { get; private set; }
    public int dodgeTotalCount { get; private set; }

    [SerializeField]
    private MatchGradeMenu winGradeMenu, loseGradeMenu;

    [Header("Gameplay Settings")]
    public PlayerSettings playerSettings;
    public TMP_Text currentDifficulty;
    public GameObject ButtonIcons;
    [SerializeField]
    private int maxMultiplier = 5;

    public static event Action OnPerfectParry;

    private Stack<int> perfectCount = new Stack<int>();
    private int pointMultiplier = 0;

    [SerializeField] private ParticleSystem comboSparklePS;
    [SerializeField] private Material comboMaterial;
    float comboIntensity = 5;
    float rampUpIntensity = 7;



    async void Start()
    {
        StopComboVFX();
        score = baseScore;
        isPlaying = true;
        await Awaitable.WaitForSecondsAsync(1.0f, CancellationToken.None);
        songLength = musicManager.timelineInfo.songLength / 1000;
    }

  

    public void Update()
    {

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

            perfectTotalCount++;

        }
        else
        {
            if(playerSettings.healOnGood == PlayerSettings.HealOnGood.Enabled)
                player.gameObject.GetComponent<Health>().Heal();
            promptImage.sprite = good;
            score += goodPoints;

            goodTotalCount++;

            ResetMultiplier();

        }
        player.inputTiming = 0;

        popupAnim.SetTrigger("playFeedback");
    }

    public void DeductScore(int val, string type)
    {
        ResetMultiplier();
        score -= val;
        if (type == "dodge")
        {
            promptImage.sprite = dodge;
            dodgeTotalCount++;
        }
        else
        {
            promptImage.sprite = miss;
            missTotalCount++;
        }
            
        
        popupAnim.SetTrigger("playFeedback");
    }

    public void CalculateFinalScore(int dodges)
    {
        StopComboVFX();

        isPlaying = false;
        score += dodges * 5;
        if (score <= 0) score = 0;
        winScore.text = "Final Score: " + Mathf.Round(score);
        loseScore.text = "Final Score: " + Mathf.Round(score);

        maxScore = (int)songLength * 4 + 15 + baseScore + (musicManager.timelineInfo.parryBeats * perfectPoints * (int)(maxMultiplier / 1.5 ));
        Debug.Log(maxScore);
        
        if (score >= maxScore * .90f) { 
            grade.sprite = gradeS;
        }
        if (score >= maxScore * .75f && score < maxScore * .90f) { 
            grade.sprite = gradeA;
    }
        if (score >= maxScore * .50f && score < maxScore * .75f) { 
            grade.sprite = gradeB;
        }
        if (score >= maxScore * .20f && score < maxScore * .50f) { 
            grade.sprite = gradeC;
        }
        if (score < maxScore * .20f) { 
            grade.sprite = gradeD;
        }
        winGradeMenu.ShowGrades();
        loseGradeMenu.ShowGrades();
       
    }

    private void IncreaseMultiplier()
    {
        perfectCount.Push(1);
        pointMultiplier = Mathf.Clamp(2 + perfectCount.Count, 1, maxMultiplier*2)/2;

        multiplierText.text = "x" + pointMultiplier.ToString();

        if (pointMultiplier == 2)
        {
            StartComboVFX();
        }

        if (pointMultiplier > 2)
        {
            ComboRampVFX(); //make success vfx more noticable
        }
    }

    private void ResetMultiplier()
    {
        perfectCount.Clear();
        pointMultiplier = 1;
        multiplierText.text = "x1";
        StopComboVFX();
    

    }

    private void StartComboVFX()
    {
       
        comboSparklePS.Play();
        comboMaterial.SetFloat("_BreathFrequency", 1);
        comboMaterial.SetFloat("_BreathIntensity", 1);

        comboMaterial.SetFloat("_VignetteIntensity", comboIntensity);
    }

    private void ComboRampVFX()
    {
        comboMaterial.SetFloat("_VignetteIntensity", rampUpIntensity);
    }

    private void StopComboVFX()
    {
       
        comboSparklePS.Stop();
        comboMaterial.SetFloat("_BreathFrequency", 0);
        comboMaterial.SetFloat("_BreathIntensity", 0);
        comboMaterial.SetFloat("_VignetteIntensity", 0);
    }


}