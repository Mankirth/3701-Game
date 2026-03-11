using UnityEngine;

public class PrincePhaseManager : PhaseEffect
{
    [SerializeField]
    private Health playerHealth;
    void Start()
    {
        playerHealth.SetHealth(1);
    }
    public override void ChangePhase(int phase)
    {
        playerHealth.healBlock = true;
    }
}
