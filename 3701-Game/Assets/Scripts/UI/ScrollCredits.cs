using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class ScrollCredits : MonoBehaviour
{

    InputAction UISubmit;
    InputAction Quit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UISubmit = InputSystem.actions.FindActionMap("UI").FindAction("Submit");
        Quit = InputSystem.actions.FindAction("Pause");
    }

    // Update is called once per frame
    void Update()
    {
        if (UISubmit.IsPressed())
        {
            Time.timeScale = 5.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        if (Quit.WasPressedThisFrame())
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
