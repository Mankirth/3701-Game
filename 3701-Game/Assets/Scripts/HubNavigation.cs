using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubNavigation : MonoBehaviour
{
    [SerializeField]
    private Transform transitionHolder, offScreenHolder, currentRoom;
    [SerializeField]
    private Animator animatior;
    private bool interactable = true;
    [SerializeField]
    private float transitionTime = 4;

    private Canvas cv;

    public GameObject titleScreenBtn;

    [SerializeField] private AudioSource buttonPress, elevator;

    private void Awake()
    {
        cv = GetComponent<Canvas>();
    }

    public void GoToRoom(Transform room)
    {
        if(!interactable)
            return;
        if(currentRoom != room)
        {
            
            currentRoom.transform.SetParent(offScreenHolder);

         

            currentRoom = room;


          

            currentRoom.SetParent(transitionHolder);
            currentRoom.SetAsLastSibling();
        }
        // titleScreenBtn.SetActive(false);
        currentRoom.gameObject.SetActive(true);
        StartCoroutine(Transition("GoToRoom"));
        

      
    }

    private IEnumerator Transition(string triggerName)
    {
        interactable = false;
        elevator.Play();
        animatior.SetTrigger(triggerName);

        yield return new WaitForSeconds(transitionTime/2);
        switch (triggerName)
        {
            case "GoToRoom":
                cv.renderMode = RenderMode.ScreenSpaceOverlay;
                
                break;
            case "BackToSelect":
                cv.renderMode = RenderMode.ScreenSpaceCamera;
                currentRoom.gameObject.SetActive(false); //ensure this room turns off after animation
                break;
        }

        yield return new WaitForSeconds(transitionTime / 2);
        animatior.ResetTrigger(triggerName);
        interactable = true;

    }

    public void BackToSelect()
    {
        buttonPress.Play();
        if(!interactable)
            return;

        titleScreenBtn.SetActive(true);
        StartCoroutine(Transition("BackToSelect"));

      
    }

    // public void LoadScene(string sceneName)
    // {

    //     SceneManager.LoadScene(sceneName);
    // }

    public void LoadScene(string sceneName)
    {
        //TODO: Keep track of the scene we came from and what fight is next
            //Keep an int of the fights done and use that to get scene instead of string
        buttonPress.Play();
        StartCoroutine(PlayAudioThenLoadScene(sceneName));
    }

    private IEnumerator PlayAudioThenLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(buttonPress.clip.length);
        SceneManager.LoadScene(sceneName);
    }
   
}
