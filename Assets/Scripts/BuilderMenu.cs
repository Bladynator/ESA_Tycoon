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
            string buttonText;
            buttonText = namesBuildings[i] + "\nPrice: " + buildingsPrefabs[i].GetComponent<BuildingMain>().price.ToString();
            if(buildingsPrefabs[i].GetComponent<BuildingMain>().rpPrice != 0)
            {
                buttonText += "\nRP: " + buildingsPrefabs[i].GetComponent<BuildingMain>().rpPrice.ToString();
            }
            allButtons[i].GetComponentInChildren<Text>().text = buttonText;
        }
        for(int i = 0; i < 5; i++)
        {
            ButtonMakingBuildings(i);
        }
    }

    void ButtonMakingBuildings(int i)
    {
        if (buildingsPrefabs[i].GetComponent<BuildingMain>().levelsNeededNewBuilding[account.amountOfEachBuilding[i]] <= account.level && buildingsPrefabs[i].GetComponent<BuildingMain>().price <= account.money && buildingsPrefabs[i].GetComponent<BuildingMain>().rpPrice <= account.researchPoints)
        {
            allButtons[i].GetComponent<Image>().color = Color.white;
            allButtons[i].onClick.AddListener(delegate { PressedBuilding(i); });
        }
        else
        {
            allButtons[i].GetComponent<Image>().color = Color.red;
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
        //reset.raycastTarget = false;
        allButtons = new Button[5];
        GameObject.Find("HUD").GetComponent<HUD>().EnableButton();
        account.ChangeColliders(true);
    }
    
    public void PlaceBuilder(int i)
    {
        Debug.Log("t");
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
