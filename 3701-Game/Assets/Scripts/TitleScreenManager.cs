using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public float bottomPosition = 0f;
    public float topPosition = 10f;
    public float panSpeed = 2f;

    [Header("UI References")]
    public GameObject clickToStartUI;
    public GameObject mainMenuUI;

    private bool isPanning = false;
    private bool hasPanned = false;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        mainCamera.transform.position = new Vector3(0f, bottomPosition, -10f);
        clickToStartUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    void Update()
    {
        if (!hasPanned && Input.GetMouseButtonDown(0))
        {
            isPanning = true;
            clickToStartUI.SetActive(false);
        }

        if (isPanning)
        {
            Debug.Log("MOOOOOOOVIIIING");
            float currentY = mainCamera.transform.position.y;
            float newY = Mathf.MoveTowards(currentY, topPosition, panSpeed * Time.deltaTime);
            mainCamera.transform.position = new Vector3(0f, newY, -10f);

            if (Mathf.Approximately(newY, topPosition))
            {
                isPanning = false;
                hasPanned = true;
                mainMenuUI.SetActive(true);
            }
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial"); // Replace with name of game scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}