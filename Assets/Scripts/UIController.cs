using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private int escapePressCount = 0;

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
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapePress();
        }
    }

    void TogglePause()
    {
        pauseMenu.style.display =
            pauseMenu.style.display == DisplayStyle.Flex
            ? DisplayStyle.None
            : DisplayStyle.Flex;
        backgroundMenus.style.display =
            backgroundMenus.style.display == DisplayStyle.Flex
            ? DisplayStyle.None
            : DisplayStyle.Flex;
    }

    void HandleEscapePress()
    {
        escapePressCount++;

        if (escapePressCount == 1)
        {
            HandleMainMenu();
        }
        else if (escapePressCount == 2)
        {
            ExecuteDoubleEscapeFunction();
        }
        else if (escapePressCount == 3)
        {
            ExecuteTripleEscapeFunction();
        }
    }

    void HandleMainMenu()
    {
        if (!isMainMenuDisplayed)
        {
            mainMenu.style.display = DisplayStyle.Flex;
            backgroundMenus.style.display = DisplayStyle.Flex;
            isMainMenuDisplayed = true;
        }
        else
        {
            mainMenu.style.display = DisplayStyle.None;
            backgroundMenus.style.display = DisplayStyle.None;
            isMainMenuDisplayed = false;
        } 
    }

    void ExecuteDoubleEscapeFunction()
    {
        exitMenu.style.display = DisplayStyle.Flex;
        mainMenu.style.display = DisplayStyle.None;
        backgroundMenus.style.display = DisplayStyle.Flex;
        isMainMenuDisplayed = false;
    }

    void ExecuteTripleEscapeFunction()
    {
        Application.Quit(); 
    }
}
