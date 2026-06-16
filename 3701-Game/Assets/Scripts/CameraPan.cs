using System.Collections;
using System.Threading;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField]
    private float panAmount = 0.2f;
    [SerializeField]
    private float dodgePanSpeed = 1.5f;
    [SerializeField]
    private float parryPanSpeed = 8.0f;

    private Vector3 startPosition;
    private Vector3 endPosition;

    [Header("Camera Positions")]
    [SerializeField]
    private Transform cameraParryLow;
    [SerializeField]
    private Transform cameraParryMed;
    [SerializeField]
    private Transform cameraParryHigh;
    [SerializeField]
    private PlayerSettings playerSettings;


    
    async void Start()
    {
        await Awaitable.WaitForSecondsAsync(9f, CancellationToken.None);
        startPosition = transform.position;
        GetComponent<Animator>().enabled = false;
        endPosition = new Vector3(startPosition.x - panAmount, startPosition.y, startPosition.z);
    }

    public IEnumerator Panning(float duration)
    {
        if (playerSettings.dodgeCamera == PlayerSettings.DodgeCamera.Enabled)
        {
            Debug.Log("DODGING PAN");
        float elapsedTime = 0;

        while (elapsedTime < duration/2)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, dodgePanSpeed * Time.deltaTime);
            yield return null;
        }

        while (elapsedTime < duration / 1.2 && elapsedTime > duration / 2)
        {
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        while (elapsedTime < duration && elapsedTime > duration / 1.2)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, startPosition, dodgePanSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = startPosition;
        }
    }

    public IEnumerator succeessPanning(float duration, string height)
    {
        if (playerSettings.parryZoom == PlayerSettings.ParryZoom.Enabled)
        {
            float elapsedTime = 0;
            Debug.Log("PAAN PROPER");
            Vector3 camParryPos = new Vector3(0, 0, 0);
            switch (height)
            {
                case ("Low"):
                    {
                        camParryPos = cameraParryLow.position;
                        break;
                    }
                case ("Med"):
                    {
                        camParryPos = cameraParryMed.position;
                        break;
                    }
                case ("High"):
                    {
                        camParryPos = cameraParryHigh.position;
                        break;
                    }
                default:
                    camParryPos = cameraParryMed.position;
                    break;
            }
            while (elapsedTime < duration / 2)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, camParryPos, parryPanSpeed * Time.deltaTime);
                yield return null;
            }

            while (elapsedTime < duration / 1.3 && elapsedTime > duration / 2)
            {
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            while (elapsedTime < duration && elapsedTime > duration / 1.3)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, startPosition, parryPanSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = startPosition;
        }
    }
}
