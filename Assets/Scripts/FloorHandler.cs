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
    void Start()
    {
        movePosition = new(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        floor = Instantiate(floorPrefab[0], gameObject.transform.position, Quaternion.identity, gameObject.transform);
        tooltip = UiDoc.GetComponent<UIDocument>().rootVisualElement.Q<Label>("Tooltip");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLevel <= 2)
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
        }
        else
            tooltip.visible = false;
        if (floor == null)
        {
            currentLevel++;
            floor = Instantiate(floorPrefab[currentLevel], gameObject.transform.position, Quaternion.identity, gameObject.transform);
            movePosition.y = 0;
        }
        Vector3 newPos = Vector3.MoveTowards(floor.transform.position, movePosition, speed * Time.deltaTime);
        floor.transform.position = newPos;
    }

    public void NextFloor()
    {
        (GameObject.Find("LevelEnd").transform.position, GameObject.Find("LevelStart").transform.position) = (GameObject.Find("LevelStart").transform.position, GameObject.Find("LevelEnd").transform.position);
        movePosition.y = 3;
    }
}
