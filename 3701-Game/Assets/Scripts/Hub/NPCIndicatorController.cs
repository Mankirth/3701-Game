using UnityEngine;

public class NPCIndicatorController : MonoBehaviour
{
    [Header("NPC Indicators")]
    [SerializeField] private GameObject swanIndicator;
    [SerializeField] private GameObject princeIndicator;

    public static int gameProgress = 0;

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
        NarrativeProgression[] narrProgs = Resources.FindObjectsOfTypeAll<NarrativeProgression>();
        if (narrProgs.Length > 0)
        {
            NarrativeProgression narrProg = narrProgs[0];
            if (narrProg.currRivalToFight == NarrativeProgression.FightableRival.Prince && gameProgress < 2)
            {
                gameProgress = 2;
            } else if (narrProg.currRivalToFight == NarrativeProgression.FightableRival.Swan && gameProgress >= 2) {
                gameProgress = 0;
            }
        }

        RefreshAllIndicators();
    }

    private void OnNPCDialogueCompleted(string npcName)
    {
        if (npcName.ToLower() == "swan" && gameProgress == 0)
        {
            gameProgress = 1;
        } else if (npcName.ToLower() == "prince" && gameProgress == 2)
        {
            gameProgress = 3;
        }

        RefreshAllIndicators();
    }

    public void RefreshAllIndicators()
    {
        if (swanIndicator != null) swanIndicator.SetActive(gameProgress == 0);
        
        if (princeIndicator != null) princeIndicator.SetActive(gameProgress == 2);
    }

    public bool AllNPCsTalked()
    {
        NarrativeProgression[] narrProgs = Resources.FindObjectsOfTypeAll<NarrativeProgression>();
        if (narrProgs.Length > 0)
        {
            NarrativeProgression narrProg = narrProgs[0];
            if (narrProg.currRivalToFight == NarrativeProgression.FightableRival.Swan)
            {
                return gameProgress >= 1;
            } else if (narrProg.currRivalToFight == NarrativeProgression.FightableRival.Prince) {
                return gameProgress >- 3;
            }
        }

        // Fall back if NarrativeProgression is not found
        if (gameProgress == 0 || gameProgress == 2) return false;
        return true;
    }
}