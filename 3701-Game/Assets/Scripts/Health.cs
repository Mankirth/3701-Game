using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        dodgesText.text = "Dodges Left: " + dodges;
    }

    public IEnumerator Hit()
    {
        dodges--;
        dodgesText.text = "Dodges Left: " + dodges;
        if (dodges >= 0){
            playerSprite.sprite = dodge;
            Debug.Log("I DODGED");
            yield return new WaitForSeconds(0.5f);
            player.ToIdle();
        }
        else
        {
            playerSprite.sprite = hurt;
            player.playerState = State.Dead;
            Debug.Log("PAIN");
            yield return new WaitForSeconds(0.5f);
            menu.EndGame(false);
        }
    }
}
