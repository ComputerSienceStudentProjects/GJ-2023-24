using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private UIDocument UIDocument;

    [SerializeField]
    private FloorStatScriptableObject floorStatObj;

    private VisualElement pauseMenu;
    private VisualElement mainMenu;
    private VisualElement exitMenu;
    private VisualElement backgroundMenus;

    private bool isMainMenuDisplayed = false;
    private bool isExitMenuDisplayed = false;

    private void Start()
    {
        pauseMenu = UIDocument.rootVisualElement.Q<VisualElement>("PauseMenu");
        mainMenu = UIDocument.rootVisualElement.Q<VisualElement>("MainMenu");
        exitMenu = UIDocument.rootVisualElement.Q<VisualElement>("ExitGame");
        backgroundMenus = UIDocument.rootVisualElement.Q<VisualElement>("Background");
    }

    void Update()
    {
        UpdateUI();
        UpdateScene();
    }

    void UpdateUI()
    {
        UIDocument.rootVisualElement.Q<Label>("floorNumber").text = "" + floorStatObj.GetFloor;
        UIDocument.rootVisualElement.Q<Label>("pointsNumber").text = "" + floorStatObj.GetScore;
        UIDocument.rootVisualElement.Q<Label>("movesNumber").text = "" + floorStatObj.GetNumberOfMoves;
    }

    void UpdateScene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleSpacePress();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapePress();
        }
    }

    void HandleSpacePress()
    {
        if (isExitMenuDisplayed)
        {
            return; // Do nothing if the exit menu is displayed :)
        }

        if (isMainMenuDisplayed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (pauseMenu.style.display == DisplayStyle.Flex)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void HandleEscapePress()
    {
        if (pauseMenu.style.display == DisplayStyle.Flex)
        {
            return; // Do nothing if the pause menu is displayed :)
        }

        if (!isMainMenuDisplayed && !isExitMenuDisplayed)
        {
            HandleMainMenu();
        }
        else if (isMainMenuDisplayed)
        {
            HandleExitMenu();
        }
        else if (isExitMenuDisplayed)
        {
            Application.Quit();
        }
    }

    void HandleMainMenu()
    {
        mainMenu.style.display = DisplayStyle.Flex;
        backgroundMenus.style.display = DisplayStyle.Flex;
        isMainMenuDisplayed = true;
    }

    void HandleExitMenu()
    {
        mainMenu.style.display = DisplayStyle.None;
        exitMenu.style.display = DisplayStyle.Flex;
        isMainMenuDisplayed = false;
        isExitMenuDisplayed = true;
    }

    void PauseGame() 
    {
        Time.timeScale = 0f;
        pauseMenu.style.display = DisplayStyle.Flex;
        backgroundMenus.style.display = DisplayStyle.Flex;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.style.display = DisplayStyle.None;
        backgroundMenus.style.display = DisplayStyle.None;
    }
}
