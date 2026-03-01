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
        StartCoroutine(Transition("GoToRoom"));
    }

    private IEnumerator Transition(string triggerName)
    {
        interactable = false;
        animatior.SetTrigger(triggerName);
        yield return new WaitForSeconds(transitionTime);
        animatior.ResetTrigger(triggerName);
        interactable = true;
    }

    public void BackToSelect()
    {
        if(!interactable)
            return;
        StartCoroutine(Transition("BackToSelect"));
    }

    public void LoadScene(string sceneName)
    {
        //TODO: Keep track of the scene we came from and what fight is next
            //Keep an int of the fights done and use that to get scene instead of string
        SceneManager.LoadScene(sceneName);
    }
}
