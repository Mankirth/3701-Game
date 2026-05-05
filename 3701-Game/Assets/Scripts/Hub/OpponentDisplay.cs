using UnityEngine;
using TMPro;
public class OpponentDisplay : MonoBehaviour
{
    [SerializeField]
    private NarrativeProgression narrativeSettings;
    [SerializeField]
    private TMP_Text rivalText;
    void Start()
    {
        rivalText.text = "Opponent:\nThe " + narrativeSettings.currRivalToFight.ToString();
    }
}
