using UnityEngine;
using UnityEngine.UI;

public class TournamentButtonController : MonoBehaviour
{
    [SerializeField] private NPCIndicatorController indicatorController;
    [SerializeField] private HubNavigation hubNavigation;
    [SerializeField] private NarrativeProgression narrProg;
    [SerializeField] private GameObject warningPromptPanel;
    [SerializeField] private string tournamentSceneName = "Tournament";
    [SerializeField] private AudioSource warning;
    

    private void Awake()
    {
        if (indicatorController == null)
            Debug.LogWarning("[TournamentButtonController] indicatorController is not assigned.");
        if (hubNavigation == null)
            Debug.LogWarning("[TournamentButtonController] hubNavigation is not assigned.");
    }

    public void OnTournamentButtonClicked()
    {
        if (indicatorController != null && indicatorController.AllNPCsTalked())
        {
            ContinueToFight();
        }
        else
        {
            if (warningPromptPanel != null)
            {
                warning.Play();
                warningPromptPanel.SetActive(true);
            }
        }
    }

    public void ContinueToFight()
    {
        if (warningPromptPanel != null)
            warningPromptPanel.SetActive(false);
        if(narrProg.currRivalToFight == NarrativeProgression.FightableRival.Prince)
            tournamentSceneName = "Prince";

        hubNavigation.LoadScene(tournamentSceneName);
    }

    public void DismissWarning()
    {
        if (warningPromptPanel != null)
            warningPromptPanel.SetActive(false);
    }
}