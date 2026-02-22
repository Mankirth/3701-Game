using UnityEngine;

public class GrayscalePostProcess : MonoBehaviour
{
    private Material material;
    [SerializeField]
    private Shader shader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
