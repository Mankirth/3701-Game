using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

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

  

    private enum CameraState { AtBottom, PanningUp, AtTop, PanningDown }
    private CameraState currentState;

  
  

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        mainCamera.transform.position = new Vector3(0f, bottomPosition, -10f);
        currentState = CameraState.AtBottom;
        UpdateUIState();
        ResetPanSpeed();
        
    }

    void Update()
    {
        HandleInput();
        UpdateCameraPosition();

    }

    public void HandleInput()
    {
        if (currentState == CameraState.AtBottom && Input.GetMouseButtonDown(0))
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
        if (currentState == CameraState.PanningUp) RampPanSpeed();
        if (currentState == CameraState.PanningDown) RampDownSpeed();

        currPanSpeed = Mathf.Clamp(currPanSpeed, 0f, 100f); //high value is arbitrary I just need it not to go to negatives
        float newY = Mathf.MoveTowards(currentY, targetPos, currPanSpeed * Time.deltaTime);
        return newY;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial"); // Replace with name of game scene
    }

    public void RampPanSpeed()
    {
        currPanSpeed += 0.25f; //mimic the SmoothDampen function cause idk why it's not working here
    }

    public void RampDownSpeed()
    {
        currPanSpeed -= 0.25f;
    }

    public void ResetPanSpeed()
    {
        currPanSpeed = panSpeed;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}