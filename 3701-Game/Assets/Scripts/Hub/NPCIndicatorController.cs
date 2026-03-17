using UnityEngine;

public class NPCIndicatorController : MonoBehaviour
{
    [SerializeField] private NPCRelationshipTracker relationshipTracker;

    // One indicator GameObject per NPC. Visible when NOT yet talked to, hidden once complete
    [SerializeField] private GameObject swanIndicator;
    [SerializeField] private GameObject princeIndicator;
    [SerializeField] private GameObject zealotIndicator;
    [SerializeField] private GameObject patriotIndicator;
    [SerializeField] private GameObject foxIndicator;
    [SerializeField] private GameObject devilIndicator;

    private void Awake()
    {
        DialogueManager.OnDialogueCompleted += OnNPCDialogueCompleted;
    }

    private void OnDestroy()
    {
        DialogueManager.OnDialogueCompleted -= OnNPCDialogueCompleted;
    }

    private void Start()
    {
        RefreshAllIndicators();
    }

    private void OnNPCDialogueCompleted(string npcName)
    {
        RefreshAllIndicators();
    }

    // Refreshes all indicator sprites based on current save data.</summary>
    public void RefreshAllIndicators()
    {
        if (relationshipTracker == null)
        {
            Debug.LogWarning("[NPCIndicatorController] relationshipTracker is not assigned.");
            return;
        }

        SetIndicator(swanIndicator,    "swan");
        // SetIndicator(princeIndicator,  "prince");
        // SetIndicator(zealotIndicator,  "zealot");
        // SetIndicator(patriotIndicator, "patriot");
        // SetIndicator(foxIndicator,     "fox");
        // SetIndicator(devilIndicator,   "devil");
    }

    public bool AllNPCsTalked()
    {
        if (relationshipTracker == null)
        {
            Debug.LogWarning("[NPCIndicatorController] relationshipTracker is not assigned.");
            return false;
        }

        return relationshipTracker.AllNPCsTalked();
    }

    private void SetIndicator(GameObject indicator, string npcName)
    {
        if (indicator == null)
            return;

        // Show indicator when NOT talked to, hide once dialogue is complete
        indicator.SetActive(!relationshipTracker.HasTalkedTo(npcName));
    }
}