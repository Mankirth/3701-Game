using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    public GameObject tutorialUI;
    public TutorialAction action;


    public enum TutorialAction
    {
        ParryHigh,
        ParryMedium,
        ParryLow,
        EngageParry,
        PressEnter
    }
}