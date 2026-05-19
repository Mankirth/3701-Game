using UnityEngine;

public class FullScreenShaderReset : MonoBehaviour
{
    [SerializeField] private Material comboMaterial;

    private void Start()
    {
        comboMaterial.SetFloat("_BreathFrequency", 0);
        comboMaterial.SetFloat("_BreathIntensity", 0);
        comboMaterial.SetFloat("_VignetteIntensity", 0);
    }
}
