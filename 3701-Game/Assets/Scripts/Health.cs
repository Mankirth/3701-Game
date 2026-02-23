using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField]
    public Sprite dodge, hurt, idle;
    private SpriteRenderer playerSprite;
    public int dodges = 3;
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
    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
        dodgesText.text = "Dodges Left: " + dodges;
    }

    public IEnumerator Hit()
    {
        dodges--;
        player.StopAllCoroutines();
        player.playerState = State.Hurting;
        dodgesText.text = "Dodges Left: " + dodges;
        if (dodges >= 0){
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
            loseSequence.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            menu.EndGame(false);
        }
    }
}
