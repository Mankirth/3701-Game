using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartConfiguration : MonoBehaviour
{
    CanvasGroup cg;
     AudioSource characterTheme;
    float dissolveTime = 2f;
    public Image NPCSword;
    public CharacterDissolve characterArt;
    public bool isOnScreen;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        characterTheme = GetComponent<AudioSource>();   
        //OnLoadScreen();
    }

    public void OffLoadScreen()
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;
        isOnScreen = false;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
      
    }

    public void OnLoadScreen()
    {
        NPCSword.color = new Color(1f, 1f, 1f, 0f); //set invisible
        cg.interactable = true;
        cg.blocksRaycasts = true;
        isOnScreen = true;
        StopAllCoroutines();
        StartCoroutine(FadeIn());

        //NPCSword.color = new Color(1f, 1f, 1f, 1f); //turn visible after coroutine
    }

    public IEnumerator FadeIn()
    {
        characterArt.StartDissolveIn();
        float elapsedTime = 0f;
       
        while (cg.alpha < 1)
        {
            elapsedTime += Time.deltaTime;
            float temp = Mathf.Lerp(0, 1, (elapsedTime / dissolveTime));
            cg.alpha = temp;
            NPCSword.color = new Color(1f, 1f, 1f, 0f); //turn invisible
            yield return null;
        }
        NPCSword.color = new Color(1f, 1f, 1f, 1f); //turn visible after coroutine

    }

    public IEnumerator FadeOut()
    {
        characterArt.StartDissolveOut();
        float elapsedTime = 0f;
        

        while (cg.alpha > 0)
        {
            elapsedTime += Time.deltaTime;
            float temp = Mathf.Lerp(1, 0, (elapsedTime / dissolveTime));
            cg.alpha = temp;
        
            yield return null;
        }

    }
}
