using UnityEngine;

public class PrincePhaseManager : PhaseEffect
{
    [SerializeField]
    private Health playerHealth;
    [SerializeField]
    private Sprite newIdle, newHigh, newMid, newLow, newStrike;
    public override void OnStart()
    {
        playerHealth.SetHealth(1, false);
    }
    public override void ChangePhase(int phase)
    {
        playerHealth.healBlock = true;
        EnemyInput enemy = GameObject.Find("Enemy").GetComponent<EnemyInput>();
        enemy.idle = newIdle;
        enemy.highParry = newHigh;
        enemy.medParry = newMid;
        enemy.lowParry = newLow;
        enemy.strike = newStrike;
    }
}
