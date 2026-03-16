using UnityEngine;

public class TournamentButtonController : MonoBehaviour
{
    [SerializeField] private NPCIndicatorController indicatorController;
    [SerializeField] private HubNavigation hubNavigation;
    [SerializeField] private GameObject warningPromptPanel;
    [SerializeField] private string tournamentSceneName = "Tournament";

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
            hubNavigation.LoadScene(tournamentSceneName);
        }
        else
        {
            if (warningPromptPanel != null)
            {
                warningPromptPanel.SetActive(true);
            }
        }
    }

    public void ContinueToFight()
    {
        if (warningPromptPanel != null)
            warningPromptPanel.SetActive(false);

        hubNavigation.LoadScene(tournamentSceneName);
    }

    public void DismissWarning()
    {
        if (warningPromptPanel != null)
            warningPromptPanel.SetActive(false);
    }
}