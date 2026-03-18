using UnityEngine;
using System.Collections;

public class PrincePhaseManager : PhaseEffect
{
    [SerializeField]
    private Health playerHealth;
    [SerializeField]
    private Sprite newIdle, newHigh, newMid, newLow, newStrike;
    [SerializeField]
    private GameObject snow, petalBurst, petals;
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
        StartCoroutine("SpriteSwap");
    }

    private IEnumerator SpriteSwap()
    {
        petalBurst.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        GameObject.Find("Enemy").GetComponent<SpriteRenderer>().sprite = newIdle;
        petals.SetActive(true);
        snow.SetActive(false);

    }
}
