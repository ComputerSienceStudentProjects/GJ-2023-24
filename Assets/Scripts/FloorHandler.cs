using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class FloorHandler : MonoBehaviour
{

    [SerializeField] private GameObject[] floorPrefab;
    [SerializeField] private GameObject UiDoc;
    [SerializeField] private float speed = .5f;
    private GameObject floor;
    private int currentLevel;
    private Vector3 movePosition;
    private Label tooltip;
    // Start is called before the first frame update
    // Initiates the first floor and adds friendly tooltip at bottom of screen
    void Start()
    {
        movePosition = new(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        floor = Instantiate(floorPrefab[0], gameObject.transform.position, Quaternion.identity, gameObject.transform);
        tooltip = UiDoc.GetComponent<UIDocument>().rootVisualElement.Q<Label>("Tooltip");
    }

    // Update is called once per frame
    // Spawns new floor as soon as old floor is destroyed, stops when currentFloor reaches the number of floors in the array.
    void Update()
    {
        if (currentLevel < floorPrefab.Length)
        {
            tooltip.visible = true;
            if (currentLevel == 0)
            {
                tooltip.text = "Click on a tile to move towards it.";
            }
            if (currentLevel == 1)
            {
                tooltip.text = "Some levels have gates, to activate them you need to move boxes into the buttons.";
            }
            if (currentLevel == 2)
            {
                tooltip.text = "That should cover the basics. Good Luck!";
            }
            if (currentLevel > 2)
            {
                tooltip.visible = false;
            }

            if (floor == null && currentLevel < floorPrefab.Length - 1)
            {
                currentLevel++;
                floor = Instantiate(floorPrefab[currentLevel], gameObject.transform.position, Quaternion.identity, gameObject.transform);
                movePosition.y = 0;

            }
            if (floor != null)
            {
                Vector3 newPos = Vector3.MoveTowards(floor.transform.position, movePosition, speed * Time.deltaTime);
                floor.transform.position = newPos;
            }
        }
        else
        {
            UiDoc.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("EndScreen").style.display = DisplayStyle.Flex;
        }
    }

    // public function called by LevelEnd.cs, swaps the end game with the start game tiles 
    // and changes the position of the floor prefab so it collides with the level clearer box collider to spawn next level
    public void NextFloor()
    {
        (GameObject.Find("LevelEnd").transform.position, GameObject.Find("LevelStart").transform.position) = (GameObject.Find("LevelStart").transform.position, GameObject.Find("LevelEnd").transform.position);
        movePosition.y = 3;
    }
}
