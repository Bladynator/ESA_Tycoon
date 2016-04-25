using UnityEngine;
using System.Collections;

public class BuildingPlacer : MonoBehaviour 
{
    public GameObject buildingToPlace, oldBuilding;
    public Vector2 activePlaceOnGrid;
    public BuildingPlacer builderPlacerTemp;
    public int fieldID;
    EmptyField[,] grid;
    bool placeAble = true;
    public bool rePos = false;
    Account account;

    void Start()
    {
        account = GameObject.Find("Account").GetComponent<Account>();
        account.ChangeColliders(false);
        grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
        for (int i = 0; i < buildingToPlace.GetComponent<BuildingMain>().size.x; i++)
        {
            for (int y = 0; y < buildingToPlace.GetComponent<BuildingMain>().size.y; y++)
            {
                Color newColor;
                if (grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].placeAble)
                {
                    newColor = Color.grey;
                }
                else
                {
                    newColor = Color.red;
                    placeAble = false;
                }
                grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].ChangeColor(newColor);
            }
        }
    }

    public void Done()
    {
        if (placeAble)
        {
            
            grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
            for (int i = 0; i < buildingToPlace.GetComponent<BuildingMain>().size.x; i++)
            {
                for (int y = 0; y < buildingToPlace.GetComponent<BuildingMain>().size.y; y++)
                {
                    grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].placeAble = false;
                }
            }

            GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].building = buildingToPlace.GetComponent<BuildingMain>().buildingName;
            account.ChangeColliders(true);
            Vector2 size = buildingToPlace.GetComponent<BuildingMain>().size;
            Vector3 newPosition = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].transform.position;
            GameObject tempBuilding = (GameObject)Instantiate(buildingToPlace, newPosition, transform.rotation);

            tempBuilding.transform.localScale = size;
            tempBuilding.GetComponent<BuildingMain>().ID = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].ID;
            tempBuilding.GetComponent<BuildingMain>().gridPosition = activePlaceOnGrid;
            Destroy(oldBuilding);
            account.UpdateAmountOFBuildings();
            account.PushSave();
            account.autoSave = true;
            GameObject.Find("HUD").GetComponent<HUD>().EnableButton();
            Destroy(gameObject);
        }
    }

    public void Delete()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
        for (int i = 0; i < buildingToPlace.GetComponent<BuildingMain>().size.x; i++)
        {
            for (int y = 0; y < buildingToPlace.GetComponent<BuildingMain>().size.y; y++)
            {
                grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].placeAble = true;
            }
        }
        GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].building = "EmptyField";
        if(account == null)
        {
            account = GameObject.Find("Account").GetComponent<Account>();
        }
        account.ChangeColliders(false);
    }

    public void ChangePosition(Vector2 newPostion)
    {
        GameObject[] allFields = GameObject.FindGameObjectsWithTag("EmptyField");
        foreach (GameObject tempField in allFields)
        {
            if (tempField.GetComponent<EmptyField>().gridPosition == newPostion)
            {
                Vector3 positionOfNewBuilding = new Vector3(tempField.transform.position.x, tempField.transform.position.y, tempField.transform.position.z);
                BuildingPlacer tempBuilding = (BuildingPlacer)Instantiate(builderPlacerTemp, positionOfNewBuilding, transform.rotation);
                tempBuilding.buildingToPlace = buildingToPlace;
                tempBuilding.activePlaceOnGrid = activePlaceOnGrid;
                tempBuilding.builderPlacerTemp = builderPlacerTemp;
                tempBuilding.fieldID = tempField.GetComponent<EmptyField>().ID;
                tempBuilding.oldBuilding = oldBuilding;
                tempBuilding.rePos = rePos;
                Destroy(gameObject);
            }
        }
        
    }
}
