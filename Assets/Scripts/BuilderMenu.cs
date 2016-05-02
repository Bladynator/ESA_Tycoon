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

        allButtons[0].onClick.AddListener(delegate { PressedBuilding(0); });
        allButtons[1].onClick.AddListener(delegate { PressedBuilding(1); });
        allButtons[2].onClick.AddListener(delegate { PressedBuilding(2); });
        allButtons[3].onClick.AddListener(delegate { PressedBuilding(3); });
        allButtons[4].onClick.AddListener(delegate { PressedBuilding(4); });
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

    void Reset()
    {
        Destroy(tempCanvas);
        allButtons = new Button[5];
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
