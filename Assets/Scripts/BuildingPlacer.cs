using UnityEngine;
using System.Collections;

public class BuildingPlacer : MonoBehaviour 
{
    public GameObject buildingToPlace, oldBuilding;
    public Vector2 activePlaceOnGrid, oldActivePlace;
    public BuildingPlacer builderPlacerTemp;
    public int fieldID;
    EmptyField[,] grid;
    public bool placeAble = true;
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
        CheckBorders();
    }

    void Update()
    {
        CheckBorders();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y);
    }

    void CheckBorders()
    {
        if (activePlaceOnGrid.x == 0)
        {
            Destroy(GameObject.Find("DownLeft"));
        }
        if (activePlaceOnGrid.y == 0)
        {
            Destroy(GameObject.Find("DownRight")); 
        }
        if (activePlaceOnGrid.x == GameObject.Find("Grid").GetComponent<Grid>().maxGridSize - buildingToPlace.GetComponent<BuildingMain>().size.x)
        {
            Destroy(GameObject.Find("TopRight"));
        }
        if (activePlaceOnGrid.y == GameObject.Find("Grid").GetComponent<Grid>().maxGridSize - buildingToPlace.GetComponent<BuildingMain>().size.y)
        {
            Destroy(GameObject.Find("TopLeft"));
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
            tempBuilding.GetComponent<BuildingMain>().taskDoing = -1;
            Destroy(oldBuilding);
            account.UpdateAmountOFBuildings();
            if (!tempBuilding.GetComponent<BuildingMain>().decoration && !rePos)
            {
                account.exp += tempBuilding.GetComponent<BuildingMain>().exp[tempBuilding.GetComponent<BuildingMain>().level];
            }
            if (GameObject.Find("Tutorial") != null)
            {
                if (!GameObject.Find("Tutorial").GetComponent<Tutorial>().tutorialDoing)
                {
                    account.PushSave();
                    account.autoSave = true;
                }
            }
            else
            {
                GameObject.Find("Account").GetComponent<Account>().PushSave();
                GameObject.Find("Account").GetComponent<Account>().autoSave = true;
            }
            GameObject.Find("HUD").GetComponent<HUD>().EnableButton();
            Destroy(gameObject);
        }
    }

    public void Delete(bool toFields = true)
    {
        string newName = "EmptyField";
        Vector2 newActivePlace = activePlaceOnGrid;
        if (!toFields)
        {
            newName = buildingToPlace.GetComponent<BuildingMain>().buildingName;
            newActivePlace = oldActivePlace;
        }
        grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
        for (int i = 0; i < buildingToPlace.GetComponent<BuildingMain>().size.x; i++)
        {
            for (int y = 0; y < buildingToPlace.GetComponent<BuildingMain>().size.y; y++)
            {
                grid[(int)newActivePlace.x + y, (int)newActivePlace.y + i].placeAble = toFields;
            }
        }
        GameObject.Find("Grid").GetComponent<Grid>().grid[(int)newActivePlace.x, (int)newActivePlace.y].building = newName;
        if(account == null)
        {
            account = GameObject.Find("Account").GetComponent<Account>();
        }
        account.ChangeColliders(false);
        GameObject.Find("HUD").GetComponent<HUD>().reset.SetActive(false);
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
                tempBuilding.oldActivePlace = oldActivePlace;
                Destroy(gameObject);
            }
        }
    }
}
