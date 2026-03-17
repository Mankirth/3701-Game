using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NarrativeProgression", menuName = "Scriptable Objects/NarrativeProgression")]
public class NarrativeProgression : ScriptableObject
{
    public FightableRival currRivalToFight = FightableRival.Swan;
    [Serializable]
    public enum FightableRival
    {
        Swan,
        Prince
    }
}
