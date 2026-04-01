using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialSettings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToHUB()
    {
        SceneManager.LoadScene("HubNavigationTest");
    }

}
