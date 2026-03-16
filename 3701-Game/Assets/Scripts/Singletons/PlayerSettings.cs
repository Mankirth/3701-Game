using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public Difficulty difficulty = Difficulty.Normal;

    [Serializable]
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public Controls controls = Controls.Default;

    public enum Controls
    {
        Default,
        Arrows,
        Numbers
    }
}
