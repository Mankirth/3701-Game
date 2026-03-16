using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelRowObject : MonoBehaviour
{

    [SerializeField] private GameObject[] panels;
   
    public AudioClip ambience;
   
    private int panelIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelIndex = 0;   
    }

    //reveal panel/dialogue text and increment index
    public AudioClip RevealPanel()
    {
        AudioClip currSFX = null;
        if (HasPanelLeft()) { currSFX = GetCurrSfx(); }

        Image currPanel = panels[panelIndex].GetComponent<Image>();

            currPanel.color = new Color(0f, 0f, 0f, 0f); //make transaprent
          
       panelIndex++;
        
        return currSFX;
    }

    public bool HasPanelLeft()
    {
        Debug.Log ("Comparing " + panelIndex + " to " + panels.Length);
        return panelIndex < panels.Length;  //if we have more things left to reveal, return true
    }

    public float GetPanelYPos()
    {
        return gameObject.transform.position.y;
    }

    public AudioClip GetAmbience()
    {
        return ambience;
    }

    public AudioClip GetCurrSfx()
    {
        Debug.Log("Fetching SFX at index " +  panelIndex);
        ComicPanelObject currPanel = panels[panelIndex].GetComponent<ComicPanelObject>();
        return currPanel.sfx;
    }
   
}
