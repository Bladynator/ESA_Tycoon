using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BuildingButtons : MonoBehaviour 
{
    string[] namesBuildings = new string[9];
    [SerializeField]
    GameObject[] buildingsPrefabs;
    [SerializeField]
    BuildingPlacer builderPlacerTemp;
    public Transform fieldLocation;
    public Vector2 fieldGridLocation;
    public int fieldID;
    Account account;
    public bool tutorialBack = false;
    Button[] allButtons = new Button[6];
    Image reset;

    void Start()
    {
        account = GameObject.Find("Account").GetComponent<Account>();
        namesBuildings = new string[buildingsPrefabs.Length];
        for (int i = 0; i < namesBuildings.Length; i++)
        {
            namesBuildings[i] = buildingsPrefabs[i].GetComponent<BuildingMain>().buildingName;
        }
        account.namesBuildings = namesBuildings;
        account.UpdateAmountOFBuildings();
        MakeButtons();
    }

    public void MakeButtons()
    {
        allButtons = GetComponentsInChildren<Button>();
        for(int i = 0; i < 3; i++)
        {
            allButtons[i].onClick.RemoveAllListeners();
        }
        reset = GameObject.Find("ResetButton").GetComponent<Image>();
        reset.raycastTarget = true;
        allButtons[0].onClick.AddListener(delegate { PressedType(0); });
        allButtons[1].onClick.AddListener(delegate { PressedType(1); });
        allButtons[2].onClick.AddListener(delegate { PressedType(2); });
        GameObject.Find("Resource").GetComponent<Button>().onClick.Invoke();
    }

    public void PressedType(int i)
    {
        tutorialBack = false;

        for (int p = 0; p < 3; p++)
        {
            string buttonText = namesBuildings[(i*3) + p] + "\nPrice: " + buildingsPrefabs[(i * 3) + p].GetComponent<BuildingMain>().moneyNeededUpgrade[0].ToString();
            if (buildingsPrefabs[(i * 3) + p].GetComponent<BuildingMain>().rpNeededUpgrade[0] != 0)
            {
                buttonText += "\nRP: " + buildingsPrefabs[(i * 3) + p].GetComponent<BuildingMain>().rpNeededUpgrade[0].ToString();
            }
            allButtons[p].GetComponentInChildren<Text>().text = buttonText;
        }
        for (int p = 0; p < 3; p++)
        {
            ButtonMakingBuildings(i * 3 + p, p);
        }
    }

    void ButtonMakingBuildings(int i, int p)
    {
        //Debug.Log(i + " / " + p);
        //Debug.Log(buildingsPrefabs[i].GetComponent<BuildingMain>().levelsNeededNewBuilding[account.amountOfEachBuilding[i]] + " / " + account.level);// + " / " + buildingsPrefabs[i].GetComponent<BuildingMain>().levelsNeededNewBuilding[account.amountOfEachBuilding[i]]);
        if (buildingsPrefabs[i].GetComponent<BuildingMain>().levelsNeededNewBuilding[account.amountOfEachBuilding[i]] <= account.level && buildingsPrefabs[i].GetComponent<BuildingMain>().moneyNeededUpgrade[0] <= account.money && buildingsPrefabs[i].GetComponent<BuildingMain>().rpNeededUpgrade[0] <= account.researchPoints)
        {
            allButtons[p].onClick.RemoveAllListeners();
            allButtons[p].GetComponent<Image>().color = Color.white;
            allButtons[p].onClick.AddListener(delegate { PressedBuilding(i); });
            allButtons[p].GetComponent<Image>().color = Color.white;
        }
        else
        {
            allButtons[p].GetComponent<Image>().color = Color.red;
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
