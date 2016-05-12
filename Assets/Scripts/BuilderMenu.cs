using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BuilderMenu : MonoBehaviour 
{
    string[] namesBuildings;
    string[] namesButtons = new string[5] { "Task Buildings", "Resource Buildings", "Decorations", "", "" };
    [SerializeField]
    GameObject[] buildingsPrefabs;
    [SerializeField]
    BuildingPlacer builderPlacerTemp;
    public Transform fieldLocation;
    public Vector2 fieldGridLocation;
    public int fieldID;
    Account account;
    public bool tutorialBack = false;
    [SerializeField]
    GameObject canvas;
    GameObject tempCanvas;
    Button[] allButtons = new Button[5];
    Image reset;

    void Start()
    {
        account = GameObject.Find("Account").GetComponent<Account>();
        namesBuildings = new string[buildingsPrefabs.Length];
        for(int i = 0; i < buildingsPrefabs.Length; i++)
        {
            namesBuildings[i] = buildingsPrefabs[i].GetComponent<BuildingMain>().buildingName;
        }
    }

    public void MakeButtons()
    {
        tempCanvas = Instantiate(canvas);
        allButtons = tempCanvas.GetComponentsInChildren<Button>();
        reset = GameObject.Find("ResetButton").GetComponent<Image>();
        reset.raycastTarget = true;
        tempCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[i].onClick.AddListener(delegate { PressedType(i); });
            allButtons[i].GetComponentInChildren<Text>().text = namesButtons[i];
        }
    }

    void PressedType(int p)
    {
        tutorialBack = false;

        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[i].GetComponentInChildren<Text>().text = namesBuildings[i] + "\nPrice: " + buildingsPrefabs[i].GetComponent<BuildingMain>().price.ToString();
        }

        if (buildingsPrefabs[0].GetComponent<BuildingMain>().levelsNeeded[account.amountOfEachBuilding[0]] <= account.level)
        {
            allButtons[0].onClick.AddListener(delegate { PressedBuilding(0); });
        }
        if (buildingsPrefabs[1].GetComponent<BuildingMain>().levelsNeeded[account.amountOfEachBuilding[1]] <= account.level)
        {
            allButtons[1].onClick.AddListener(delegate { PressedBuilding(1); });
        }
        if (buildingsPrefabs[2].GetComponent<BuildingMain>().levelsNeeded[account.amountOfEachBuilding[2]] <= account.level)
        {
            allButtons[2].onClick.AddListener(delegate { PressedBuilding(2); });
        }
        if (buildingsPrefabs[3].GetComponent<BuildingMain>().levelsNeeded[account.amountOfEachBuilding[3]] <= account.level)
        {
            allButtons[3].onClick.AddListener(delegate { PressedBuilding(3); });
        }
        if (buildingsPrefabs[4].GetComponent<BuildingMain>().levelsNeeded[account.amountOfEachBuilding[4]] <= account.level)
        {
            allButtons[4].onClick.AddListener(delegate { PressedBuilding(4); });
        }
    }

    void PressedBuilding(int i)
    {
        PlaceBuilder(i);
        tutorialBack = false;
        if (GameObject.Find("Tutorial") != null)
        {
            GameObject.Find("Tutorial").GetComponent<Tutorial>().DestroyArrow();
        }
    }

    public GameObject[] GetAllBuildings()
    {
        return buildingsPrefabs;
    }

    public void Reset()
    {
        Destroy(tempCanvas);
        reset.raycastTarget = false;
        allButtons = new Button[5];
        GameObject.Find("HUD").GetComponent<HUD>().EnableButton();
        account.ChangeColliders(true);
    }
    
    public void PlaceBuilder(int i)
    {
        Reset();
        Vector3 positionOfNewBuilding = new Vector3(fieldLocation.position.x, fieldLocation.position.y, fieldLocation.position.z);
        BuildingPlacer tempBuilding = (BuildingPlacer)Instantiate(builderPlacerTemp, positionOfNewBuilding, transform.rotation);
        tempBuilding.buildingToPlace = buildingsPrefabs[i];
        tempBuilding.activePlaceOnGrid = fieldGridLocation;
        tempBuilding.builderPlacerTemp = builderPlacerTemp;
        tempBuilding.fieldID = fieldID;
        account.ChangeColliders(false);
        gameObject.SetActive(false);
    }
}
