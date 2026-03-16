using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialNav : MonoBehaviour
{
    public void GoToScene(string key) {

        SceneManager.LoadScene(key);    
        }
}
