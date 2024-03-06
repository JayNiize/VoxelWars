using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuContainer;
    [SerializeField] private GameObject settingsContainer;

    private void Start()
    {
        mainMenuContainer.SetActive(true);
        settingsContainer.SetActive(false);
    }

    public void NavigateToSettings()
    {
        mainMenuContainer.SetActive(false);
        settingsContainer.SetActive(true);
    }

    public void NavigateToMainMenu()
    {
        mainMenuContainer.SetActive(true);
        settingsContainer.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}