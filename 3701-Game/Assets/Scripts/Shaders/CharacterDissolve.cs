using System.Collections;
using UnityEngine;

public class CharacterDissolve : MonoBehaviour
{
    public float dissolveDuration = 2f;
    public float dissolveStrength;
    public Material dissolveMaterial;
    
    public void StartDissolveIn()
    {
        StartCoroutine(DissolveIn());
    }

    public void StartDissolveOut()
    {
        StopCoroutine(DissolveOut());   
    }
   public IEnumerator DissolveIn()
    {
        float elapsedTime = 0;

       

        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            dissolveStrength = Mathf.Lerp(1, 0, elapsedTime / dissolveDuration);
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveStrength);

            yield return null;
        }

    }

    public IEnumerator DissolveOut()
    {
        float elapsedTime = 0;

     

        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            dissolveStrength = Mathf.Lerp(0, 1, elapsedTime / dissolveDuration);
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveStrength);

            yield return null;
        }

    }
}
