using System.Collections;
using UnityEngine;

public class LoseSequence : MonoBehaviour
{
    private readonly int colourSteps = 25;
    private Color originalColor;
    [SerializeField]
    private SpriteRenderer sprite1, sprite2;
    private Transform camera;
    private Vector3 origCamPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        originalColor = sprite1.color;
        sprite1.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        sprite2.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        camera = Camera.main.transform;
        origCamPos = camera.transform.position;
        StartCoroutine(Lose());
    }

    private IEnumerator Lose()
    {
        GameObject.Find("Player").GetComponent<SpriteRenderer>().color = Color.black;
        GameObject.Find("Enemy").GetComponent<SpriteRenderer>().color = Color.black;
        for(float i = 0.4f; i > 0; i--)
        {
            camera.position = new Vector3(Random.Range(-i, i),Random.Range(-i, i),Random.Range(-i, i)) + origCamPos;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        camera.position = origCamPos;
        for(float i = colourSteps; i > 0; i--)
        {
            sprite1.color = new Color(originalColor.r, originalColor.g, originalColor.b, (float)i / colourSteps);
            sprite2.color = new Color(originalColor.r, originalColor.g, originalColor.b, (float)i / colourSteps);

            float val = (colourSteps - i) / colourSteps;
            GameObject.Find("Enemy").GetComponent<SpriteRenderer>().color = new Color(val, val, val);
            GameObject.Find("Player").GetComponent<SpriteRenderer>().color = new Color(val, val, val);

            camera.position = new Vector3(Random.Range(-0.1f, 0.1f),Random.Range(-0.1f, 0.1f),Random.Range(-0.1f, 0.1f)) + origCamPos;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        gameObject.SetActive(false);
    }
}
