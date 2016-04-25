using UnityEngine;
using System.Collections;

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
    int screenForBuilding = 0; // 0 = main, 1 = buildings
    [SerializeField]
    GUISkin redFont;
    [SerializeField]
    GUISkin standard;

    void Start()
    {
        account = GameObject.Find("Account").GetComponent<Account>();
        namesBuildings = new string[buildingsPrefabs.Length];
        for(int i = 0; i < buildingsPrefabs.Length; i++)
        {
            namesBuildings[i] = buildingsPrefabs[i].GetComponent<BuildingMain>().buildingName;
        }
    }

    public GameObject[] GetAllBuildings()
    {
        return buildingsPrefabs;
    }

    void OnGUI()
    {
        switch (screenForBuilding)
        {
            case 0:
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (GUI.Button(new Rect(0 + (i * Screen.width / 5), Screen.height - Screen.height / 4, Screen.width / 5, Screen.height / 4), namesButtons[i]))
                        {
                            screenForBuilding = (i + 1);
                        }
                    }
                    break;
                }
            case 1:
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (buildingsPrefabs[i].GetComponent<BuildingMain>().levelNeeded <= account.level)
                        {
                            if (GUI.Button(new Rect(0 + (i * Screen.width / 5), Screen.height - Screen.height / 4, Screen.width / 5, Screen.height / 4), namesBuildings[i] + "\nPrice: " + buildingsPrefabs[i].GetComponent<BuildingMain>().price.ToString()))
                            {
                                Vector3 positionOfNewBuilding = new Vector3(fieldLocation.position.x, fieldLocation.position.y, fieldLocation.position.z);
                                BuildingPlacer tempBuilding = (BuildingPlacer)Instantiate(builderPlacerTemp, positionOfNewBuilding, transform.rotation);
                                tempBuilding.buildingToPlace = buildingsPrefabs[i];
                                tempBuilding.activePlaceOnGrid = fieldGridLocation;
                                tempBuilding.builderPlacerTemp = builderPlacerTemp;
                                tempBuilding.fieldID = fieldID;
                                account.ChangeColliders(false);
                                gameObject.SetActive(false);
                                screenForBuilding = 0;
                            }
                        }
                        else
                        {
                            GUI.skin = redFont;
                            GUI.Button(new Rect(0 + (i * Screen.width / 5), Screen.height - Screen.height / 4, Screen.width / 5, Screen.height / 4), namesBuildings[i] + "\nLevel Needed: " + buildingsPrefabs[i].GetComponent<BuildingMain>().levelNeeded.ToString());
                            GUI.skin = standard;
                        }
                    }
                    break;
                }
        }
        if (GUI.Button(new Rect(0, Screen.height - Screen.height / 4 - 50, 50, 50), "Back"))
        {
            if (screenForBuilding != 0)
            {
                screenForBuilding = 0;
            }
            else
            {
                GameObject.Find("HUD").GetComponent<HUD>().EnableButton();
                gameObject.SetActive(false);
            }
        }
    }
}
