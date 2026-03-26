using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    [SerializeField]
    private PhaseEffect phaseHandler;
    [Header("UI Elements")]
    public TMP_Text phaseText;
    public Animation menuAnim;
    public List<string> phases = new List<string>(); // Switch to gameobject or sprite when we have text for this
    [HideInInspector]
    public bool changingPhase = false;
    private GameMenu menu;
    void Start()
    {
        phaseHandler.OnStart();
        gameObject.SetActive(false);
        menu = GameObject.Find("HUD Canvas").GetComponent<GameMenu>();
    }
    public IEnumerator ShowPhase()
    {
        phaseText.text = phases[0];
        menuAnim.Play();
        phaseHandler.ChangePhase(1);
        yield return new WaitForSeconds(2.5f);
        changingPhase = false;
        phases.RemoveAt(0);
        menu.pausable = true;
        gameObject.SetActive(false);
    }
    
}
