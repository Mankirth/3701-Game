using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    public Sprite dodge, hurt, idle;
    public Image[] dodgeHearts;
    private SpriteRenderer playerSprite;
    public int dodges = 3;
    private int healInc = 0, maxDodges = 3;
    [SerializeField]
    private PlayerInput player;
    [SerializeField]
    private TMP_Text dodgesText;

    public GameMenu menu;
    public GameManager gameManager;
    private SfxManager sfxManager;
    [SerializeField]
    private GameObject loseSequence;

    public Transform dodgePos, defaultPos;
    public bool healBlock;
    private bool canHeal = true;
    private readonly int healSteps = 5;
    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
        dodgesText.text = "Dodges Left: " + dodges;
    }

    public IEnumerator Hit()
    {
        if(dodges != dodgeHearts.Length)
            dodgeHearts[dodges].fillAmount = 0;
        dodges--;
        player.StopAllCoroutines();
        player.playerState = State.Hurting;
        dodgesText.text = "Dodges Left: " + dodges;
        healInc = 0;
        if (healBlock)
        {
            StopAllCoroutines();
            StartCoroutine("BlockHealing");
        }
      
        if (dodges >= 0){
            dodgeHearts[dodges].fillAmount = 0;
            dodgeHearts[dodges].color = Color.gray; //black out dodge hearts to indicate dodges left
            sfxManager.QueueSound(false, sfxManager.playerDodge);
            playerSprite.sprite = dodge;
            Debug.Log("I DODGED");
            transform.position = dodgePos.position;
            yield return new WaitForSeconds(0.5f);
            transform.position = defaultPos.position;
            player.ToIdle();
        }
        else
        {
            sfxManager.QueueSound(false, sfxManager.enemyHit); //REPLACE WITH PLAYER HIT
            playerSprite.sprite = hurt;
            Debug.Log("PAIN");
            Time.timeScale = 0.1f;
            menu.hud.SetActive(false);
            loseSequence.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            menu.EndGame(false);
        }
    }

    public void Heal()
    {
        if(dodges >= maxDodges || !canHeal)
            return;
        healInc++;
        dodgeHearts[dodges].fillAmount = (float)healInc / healSteps;
        if(healInc >= healSteps)
        {
            healInc = 0;
            dodgeHearts[dodges].color = Color.white;
            dodges++;
        }
    }

    public void SetHealth(int health, bool heal)
    {
        if((heal && dodges <= health) || (!heal && dodges >= health))
            dodges = health;
        int i = 0;
        foreach(Image heart in dodgeHearts)
        {
            if(i < health){
                if(heal){
                    heart.fillAmount = 1;
                    heart.color = Color.white;
                }
            }
            else{
                heart.fillAmount = 0;
                heart.color = Color.gray;
            }
            i++;
        }
    }

    private IEnumerator BlockHealing()
    {
        canHeal = false;
        yield return new WaitForSeconds(10);
        canHeal = true;
    }
}
