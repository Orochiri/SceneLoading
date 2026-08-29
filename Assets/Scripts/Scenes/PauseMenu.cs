using System;
using System.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject container;
    private InputSystem_Actions _pauseInput;
    private bool menuActive;
    
    void Start()
    {
        _pauseInput = new InputSystem_Actions();
        _pauseInput.Enable();
        
        _pauseInput.UI.Pause.performed += OnEscapeButton;
    }

    private void OnDisable()
    {
        _pauseInput.UI.Pause.performed -= OnEscapeButton;
    }

    private void OnEscapeButton(InputAction.CallbackContext obj)
    {
        ToggleMenu();
    }

    private void ToggleMenu()
    {
        if (menuActive)
        {
            menuActive = false;
            container.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            menuActive = true;
            container.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeButton()
    {
        container.SetActive((false));
        Time.timeScale = 1;
    }
    
    public void MainMenuButton()
    {
        container.SetActive((false));
        Time.timeScale = 1;
        SceneLoader.Instance.LoadScene(0);
    }
}
