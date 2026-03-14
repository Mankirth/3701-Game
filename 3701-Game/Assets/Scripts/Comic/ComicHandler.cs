using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.SceneManagement;

public class ComicHandler : MonoBehaviour
{
    public Material dissolveMaterial;
    private float dissolveDuration = 1f;
    private float dissolveStrength;

    public string ConnectingScene;
    public PanelRowObject[] rows;
    private int currIndex;
    private float panDuration = 3f;
    public Camera cam;

     private enum CameraState {AtBottom, PanningDown, Progressing}
    [SerializeField] private CameraState state;

    private void Start()
    {
        StartCoroutine(DissolveIn());
        currIndex = 0;
        state = CameraState.Progressing;
        UpdateCameraState();
        rows[currIndex].RevealPanel(); //reveal the first panel
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    public void HandleInput()
    {
        UpdateCameraState();
        

        switch (state)
        {
            case CameraState.AtBottom:
                StartCoroutine(DissolveOut());
               
                Debug.Log("We are at the bottom");
                break;
            case CameraState.Progressing:
                TraverseRow();
                break;
        }
    }

    public void TraverseRow()
    {
       

        //1. Check if we have a panel to reveal, then reveal it
        if (rows[currIndex].HasPanelLeft())
        {
            rows[currIndex].RevealPanel();
        } else //2. We have no more panels to reveal, move to next row
        {
            currIndex++;
            if (currIndex < rows.Length) //need to case check after to ensure we don't trigger index out of range
            {
                if (currIndex > 0) MoveCamera(); // don't want to move camera to the position at the top
                rows[currIndex].RevealPanel(); //reveal the first panel afters crolling down
            } 

        }
      

        
    }

   

    public void MoveCamera()
    {
        Debug.Log("Moving panel at index " + currIndex + " which is located at " + rows[currIndex].GetPanelYPos());

        state = CameraState.PanningDown;
        StartCoroutine(LerpCamera(rows[currIndex].GetPanelYPos()));

     

    }
  

    public void UpdateCameraState()
    {
        if (currIndex < rows.Length && state!=CameraState.PanningDown) state = CameraState.Progressing; //not done getting throug comic, and also not transitioning
        else if (currIndex >= rows.Length) state = CameraState.AtBottom; //we are done with the comic
    }

    private IEnumerator LerpCamera(float targetPosY)
    {
     
        float timeElapsed = 0f;
        Vector3 startPos = cam.transform.position;
        Vector3 targetPos = new Vector3(cam.transform.position.x, targetPosY, cam.transform.position.z);
        while (timeElapsed < panDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, timeElapsed / panDuration);
            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = targetPos; //ensure camera is set to location at end
        yield return new WaitForSeconds(0.25f); //buffer
        state = CameraState.Progressing; //prevent player from proceeding before camera finishes moving
    }

    public IEnumerator DissolveOut()
    {
        float elapsedTime = 0;



        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            dissolveStrength = Mathf.Lerp(1, 0, elapsedTime / dissolveDuration);
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveStrength);

            yield return null;
        }
        SceneManager.LoadScene(ConnectingScene); //proceed to next scene
     
    }

    public IEnumerator DissolveIn()
    {
        float elapsedTime = 0;



        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            dissolveStrength = Mathf.Lerp(0, 1, elapsedTime / dissolveDuration);
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveStrength);

            yield return null;
        }
        yield return new WaitForSeconds(0.5f); //buffer

    }
}
