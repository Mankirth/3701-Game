using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class TitleScreenController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public float bottomPosition = 0f;
    public float topPosition = 10f;
    const float panSpeed = 0.01f;
    public float currPanSpeed;
    

    [Header("UI References")]
    public GameObject clickToStartUI;
    public GameObject mainMenuUI;


    [Header("Audio")]
    public AudioSource buttonPress;

    [Header("Player Settings")]
    public PlayerSettings settings;

    public NarrativeProgression progressionManager;


    private enum CameraState { AtBottom, PanningUp, AtTop, PanningDown }
    private CameraState currentState;

    public InputActionAsset UIControls;

    InputAction UISubmit;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        settings.ResetToDefault();
        mainCamera.transform.position = new Vector3(0f, bottomPosition, -10f);
        currentState = CameraState.AtBottom;
        UpdateUIState();
        ResetPanSpeed();
        progressionManager.ResetGame();

        UISubmit = UIControls.FindActionMap("UI").FindAction("Submit");
        
    }

    void Update()
    {
        HandleInput();
        UpdateCameraPosition();

    }

    public void HandleInput()
    {
        if (currentState == CameraState.AtBottom && UISubmit.WasPressedThisFrame() || Input.GetMouseButton(0))
        {
            currentState = CameraState.PanningUp;
            UpdateUIState();

        }
        else if (currentState == CameraState.AtTop && Input.GetKeyDown(KeyCode.Escape))
        {
            currentState = CameraState.PanningDown;
            UpdateUIState();

        }

      
    }
    public void UpdateCameraPosition()
    {

       

        if (currentState == CameraState.PanningUp)
        {

          
            float newY = Pan(mainCamera.transform.position.y, topPosition);

            mainCamera.transform.position = new Vector3(0f, newY, -10f);

                if (Mathf.Approximately(newY, topPosition))
                {
                     currentState = CameraState.AtTop;
                    UpdateUIState();
                    ResetPanSpeed();

               }
            }
            else if (currentState == CameraState.PanningDown)
            {

          
            float newY = Pan(mainCamera.transform.position.y, bottomPosition);

            mainCamera.transform.position = new Vector3(0, newY, -10f);

                if (Mathf.Approximately(newY, bottomPosition))
                {
                    currentState = CameraState.AtBottom;
                    UpdateUIState();
                    ResetPanSpeed();


            }
            }





        }
    

    public void UpdateUIState()
    {
        clickToStartUI.SetActive(currentState == CameraState.AtBottom);
        mainMenuUI.SetActive(currentState == CameraState.AtTop);
    }

    public float Pan(float currentY, float targetPos)
    {
        RampPanSpeed();
        float newY = Mathf.MoveTowards(currentY, targetPos, currPanSpeed * Time.deltaTime);
        return newY;
    }
    public void PlayGame()
    {
        buttonPress.Play();
        StartCoroutine(PlayAudioThenLoadIntro()); // Comic introduction cutscene
    }

    private IEnumerator PlayAudioThenLoadIntro()
    {
        yield return new WaitForSeconds(buttonPress.clip.length);
        SceneManager.LoadScene("Intro");
    }

    public void RampPanSpeed()
    {
        currPanSpeed += 0.10f; //mimic the SmoothDampen function cause idk why it's not working here
    }

    public void ResetPanSpeed()
    {
        currPanSpeed = panSpeed;
    }
    public void QuitGame()
    {
        buttonPress.Play();
        Application.Quit();
    }
}