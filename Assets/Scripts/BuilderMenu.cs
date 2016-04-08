using UnityEngine;
using System.Collections;

public class BuilderMenu : MonoBehaviour 
{
    string[] names = new string[5] { "engineer", "builder", "teamleader", "headquaters", "testingroom"};
    [SerializeField]
    GameObject[] buildingsPrefabs;
    [SerializeField]
    BuildingPlacer builderPlacerTemp;
    public Transform fieldLocation;
    public Vector2 fieldGridLocation;
    public int fieldID;
	
    void OnGUI()
    {
        for(int i = 0; i < 5; i++)
        {
            if(GUI.Button(new Rect(0 + (i * Screen.width / 5), Screen.height - Screen.height / 4, Screen.width / 5, Screen.height / 4), names[i]))
            {
                Vector3 positionOfNewBuilding = new Vector3(fieldLocation.position.x, fieldLocation.position.y + 0.3f, fieldLocation.position.z);
                BuildingPlacer tempBuilding = (BuildingPlacer)Instantiate(builderPlacerTemp, positionOfNewBuilding, transform.rotation);
                tempBuilding.buildingToPlace = buildingsPrefabs[i];
                tempBuilding.activePlaceOnGrid = fieldGridLocation;
                tempBuilding.builderPlacerTemp = builderPlacerTemp;
                tempBuilding.fieldID = fieldID;
                gameObject.SetActive(false);
            }
        }
    }
}
