using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    public Sprite dodge, hurt, idle;
    private SpriteRenderer playerSprite;

    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
    }
    public void Dodge() // Change to IEnumerator, (Should dodge action be instant or slowed slightly?)
    {
        playerSprite.sprite = dodge;
        Debug.Log("I DODGED");
    }

    public void Hurt()
    {
        playerSprite.sprite = hurt;
        Debug.Log("I HURT AHHH");
    }
}
