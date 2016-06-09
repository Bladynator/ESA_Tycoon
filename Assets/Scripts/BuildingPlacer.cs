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
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject tempBuilding in allBuildings)
        {
            tempBuilding.GetComponent<CircleCollider2D>().enabled = false;
            tempBuilding.GetComponent<BuildingMain>().canClick = false;
            GameObject.Find("Main Camera").GetComponent<CameraChanger>().builderOn = true;
        }
        //transform.localScale = buildingToPlace.GetComponent<BuildingMain>().size;
    }

    void Update()
    {
        CheckBorders();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y);
        Transform[] Arrows = GetComponentsInChildren<Transform>();
        int tempSize = (int)buildingToPlace.GetComponent<BuildingMain>().size.x;
        switch (tempSize)
        {
            case 1:
                {
                    Arrows[1].localPosition = new Vector2(1.33f, 1.02f);
                    Arrows[2].localPosition = new Vector2(-1.27f, 1f);
                    Arrows[3].localPosition = new Vector2(1.35f, -0.37f);
                    Arrows[4].localPosition = new Vector2(-1.29f, -0.38f);
                    Arrows[5].localPosition = new Vector2(0.85f, 2.08f);
                    Arrows[6].localPosition = new Vector2(-0.66f, 2.08f);
                    break;
                }
            case 2:
                {
                    Arrows[1].localPosition = new Vector2(1.33f, 1.02f);
                    Arrows[2].localPosition = new Vector2(-1.27f, 1f);
                    Arrows[3].localPosition = new Vector2(1.35f, -0.37f);
                    Arrows[4].localPosition = new Vector2(-1.29f, -0.38f);
                    Arrows[5].localPosition = new Vector2(0.85f, 2.08f);
                    Arrows[6].localPosition = new Vector2(-0.66f, 2.08f);
                    break;
                }
            case 3:
                {
                    Arrows[1].localPosition = new Vector2(1.6f, 1.6f);
                    Arrows[2].localPosition = new Vector2(-1.6f, 1.6f);
                    Arrows[3].localPosition = new Vector2(1.6f, -0.2f);
                    Arrows[4].localPosition = new Vector2(-1.6f, -0.2f);
                    Arrows[5].localPosition = new Vector2(0.85f, 2.81f);
                    Arrows[6].localPosition = new Vector2(-0.66f, 2.81f);
                    break;
                }
        }
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
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject tempBuilding in allBuildings)
        {
            tempBuilding.GetComponent<CircleCollider2D>().enabled = true;
            tempBuilding.GetComponent<BuildingMain>().canClick = true;
            GameObject.Find("Main Camera").GetComponent<CameraChanger>().builderOn = false;
        }
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

            tempBuilding.transform.localScale = new Vector2(size.x * 0.98f, size.y * 0.95f);
            tempBuilding.GetComponent<BuildingMain>().ID = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].ID;
            tempBuilding.GetComponent<BuildingMain>().gridPosition = activePlaceOnGrid;
            tempBuilding.GetComponent<BuildingMain>().taskDoing = -1;
            Destroy(oldBuilding);
            if (!tempBuilding.GetComponent<BuildingMain>().decoration && !rePos)
            {
                account.exp += tempBuilding.GetComponent<BuildingMain>().exp[tempBuilding.GetComponent<BuildingMain>().level];
                GameObject.Find("HUD").GetComponent<HUD>().notificationNumber--;
                GameObject.Find("HUD").GetComponent<HUD>().UpdateNotification();
            }
            account.UpdateAmountOFBuildings();
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
