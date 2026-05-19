using UnityEngine;

public class FullscreenShaderManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Material comboMaterial;
    void Start()
    {
        comboMaterial.SetFloat("_BreathFrequency", 0);
        comboMaterial.SetFloat("_BreathIntensity", 0);
        comboMaterial.SetFloat("_VignetteIntensity", 0);
    }

    
}
