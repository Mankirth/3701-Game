using System.Collections;
using UnityEngine;

public class LoseSequence : MonoBehaviour
{
    private readonly int colourSteps = 25;
    private Color originalColor;
    [SerializeField]
    private SpriteRenderer sprite1, sprite2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        originalColor = sprite1.color;
        sprite1.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        sprite2.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        StartCoroutine(Lose());
    }

    private IEnumerator Lose()
    {
        GameObject.Find("Player").GetComponent<SpriteRenderer>().color = Color.black;
        GameObject.Find("Enemy").GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSecondsRealtime(0.4f);
        for(float i = colourSteps; i > 0; i--)
        {
            Debug.Log("Lose LOOP RUNNING");
            sprite1.color = new Color(originalColor.r, originalColor.g, originalColor.b, (float)i / colourSteps);
            sprite2.color = new Color(originalColor.r, originalColor.g, originalColor.b, (float)i / colourSteps);
            float val = (colourSteps - i) / colourSteps;
            Debug.Log(val);
            GameObject.Find("Enemy").GetComponent<SpriteRenderer>().color = new Color(val, val, val);
            GameObject.Find("Player").GetComponent<SpriteRenderer>().color = new Color(val, val, val);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        gameObject.SetActive(false);
    }
}
