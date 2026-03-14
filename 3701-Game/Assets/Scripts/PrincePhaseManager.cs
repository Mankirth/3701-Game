using UnityEngine;

public class PrincePhaseManager : PhaseEffect
{
    [SerializeField]
    private Health playerHealth;
    public override void OnStart()
    {
        playerHealth.SetHealth(1, false);
    }
    public override void ChangePhase(int phase)
    {
        playerHealth.healBlock = true;
    }
}
