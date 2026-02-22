using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArtConfiguration : MonoBehaviour
{
    CanvasGroup cg;

    float dissolveTime = 2f;
    public Image NPCSword;
    public CharacterDissolve characterArt;
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        OnLoadScreen();
    }

    public void OffLoadScreen()
    {
        
        StartCoroutine(FadeOut());
      
    }

    public void OnLoadScreen()
    {
        NPCSword.color = new Color(1f, 1f, 1f, 0f); //set invisible

        StartCoroutine(FadeIn());

        NPCSword.color = new Color(1f, 1f, 1f, 1f); //turn visible after coroutine
    }

    public IEnumerator FadeIn()
    {
        characterArt.StartDissolveIn();
        float elapsedTime = 0f;

        cg.interactable = true;
        cg.blocksRaycasts = true;
       
        while (cg.alpha < 1)
        {
            elapsedTime += Time.deltaTime;
            float temp = Mathf.Lerp(0, 1, (elapsedTime / dissolveTime));
            cg.alpha = temp;
            NPCSword.color = new Color(1f, 1f, 1f, 255); //turn invisible
            yield return null;
        }

    }

    public IEnumerator FadeOut()
    {
        characterArt.StartDissolveOut();
        float elapsedTime = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        

        while (cg.alpha > 0)
        {
            elapsedTime += Time.deltaTime;
            float temp = Mathf.Lerp(1, 0, (elapsedTime / dissolveTime));
            cg.alpha = temp;
        
            yield return null;
        }

    }
}
