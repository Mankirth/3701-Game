
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SwanPhaseHandler : PhaseEffect
{
    [SerializeField]
    private Volume globalVol;
    private ColorAdjustments colorAdjustments;
    private float desaturateTime = 2;
    public override void ChangePhase(int phase)
    {
        StartCoroutine("Desaturate");
    }

    private IEnumerator Desaturate()
    {
        if (globalVol != null){
            globalVol.profile.TryGet<ColorAdjustments>(out colorAdjustments);
            for(float i = 0; i < desaturateTime; i += Time.deltaTime)
            {
                colorAdjustments.saturation.value = -100f * (i / desaturateTime);
                yield return null;
            }
            colorAdjustments.saturation.value = -100f;
        }
    }
}
