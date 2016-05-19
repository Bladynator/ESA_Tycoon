using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BuildingButtons : MonoBehaviour 
{
    string[] namesBuildings;
    [SerializeField]
    GameObject[] buildingsPrefabs;
    [SerializeField]
    BuildingPlacer builderPlacerTemp;
    public Transform fieldLocation;
    public Vector2 fieldGridLocation;
    public int fieldID;
    Account account;
    public bool tutorialBack = false;
    Button[] allButtons = new Button[5];
    Image reset;

    void Start()
    {
        account = GameObject.Find("Account").GetComponent<Account>();
        namesBuildings = new string[buildingsPrefabs.Length];
        for (int i = 0; i < buildingsPrefabs.Length; i++)
        {
            namesBuildings[i] = buildingsPrefabs[i].GetComponent<BuildingMain>().buildingName;
        }
        
        MakeButtons();
    }

    public void MakeButtons()
    {
        allButtons = GetComponentsInChildren<Button>();
        reset = GameObject.Find("ResetButton").GetComponent<Image>();
        reset.raycastTarget = true;
        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[i].onClick.AddListener(delegate { PressedType(i); });
        }
        GameObject.Find("Resource").GetComponent<Button>().onClick.Invoke();
    }

    public void PressedType(int p)
    {
        tutorialBack = false;

        for (int i = 0; i < allButtons.Length; i++)
        {
            string buttonText;
            buttonText = namesBuildings[i] + "\nPrice: " + buildingsPrefabs[i].GetComponent<BuildingMain>().price.ToString();
            if (buildingsPrefabs[i].GetComponent<BuildingMain>().rpPrice != 0)
            {
                buttonText += "\nRP: " + buildingsPrefabs[i].GetComponent<BuildingMain>().rpPrice.ToString();
            }
            allButtons[i].GetComponentInChildren<Text>().text = buttonText;
        }
        for (int i = 0; i < 3; i++)
        {
            ButtonMakingBuildings(i);
        }
    }

    void ButtonMakingBuildings(int i)
    {
        if (buildingsPrefabs[i].GetComponent<BuildingMain>().levelsNeeded[account.amountOfEachBuilding[i]] <= account.level && buildingsPrefabs[i].GetComponent<BuildingMain>().price <= account.money && buildingsPrefabs[i].GetComponent<BuildingMain>().rpPrice <= account.researchPoints)
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

    public void PlaceBuilder(int i)
    {
        GameObject obj = GameObject.Find("Main Camera").GetComponent<CameraChanger>().Field();
        if (obj != null)
        {
            if (obj.GetComponent<EmptyField>() != null)
            {
                fieldLocation = obj.GetComponent<EmptyField>().transform;
                fieldID = obj.GetComponent<EmptyField>().ID;
                fieldGridLocation = obj.GetComponent<EmptyField>().gridPosition;
            }
        }
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
