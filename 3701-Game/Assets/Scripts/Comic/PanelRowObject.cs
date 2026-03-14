using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelRowObject : MonoBehaviour
{

    [SerializeField] private Image[] coverPanel;
   
    private int panelIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panelIndex = 0;   
    }

    //reveal panel/dialogue text and increment index
    public void RevealPanel()
    {

            coverPanel[panelIndex].color = new Color(0f, 0f, 0f, 0f); //make transaprent
            panelIndex++;
    
    }

    public bool HasPanelLeft()
    {
        return panelIndex < coverPanel.Length;  //if we have more things left to reveal, return true
    }

    public float GetPanelYPos()
    {
        return gameObject.transform.position.y;
    }
   
}
