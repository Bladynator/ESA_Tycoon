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
    [SerializeField]
    Transform[] Arrows;

    void Start()
    {
        account = GameObject.Find("Account").GetComponent<Account>();
        //account.ChangeColliders(false);
        MainGameController.ChangeColliders(false);
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
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject tempBuilding in allBuildings)
        {
            tempBuilding.GetComponent<CircleCollider2D>().enabled = false;
            tempBuilding.GetComponent<BuildingMain>().canClick = false;
            GameObject.Find("Main Camera").GetComponent<CameraChanger>().builderOn = true;
        }
        CheckBorders();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y);
        int tempSize = (int)buildingToPlace.GetComponent<BuildingMain>().size.x;
        switch (tempSize)
        {
            case 1:
                {
                    GiveArrowPosition(0, 0.916f, 0.513f);
                    GiveArrowPosition(1, -0.916f, 0.513f);
                    GiveArrowPosition(2, 0.916f, -0.508f);
                    GiveArrowPosition(3, -0.916f, -0.508f);
                    GiveArrowPosition(4, 0.85f, 2.08f);
                    GiveArrowPosition(5, -0.66f, 2.08f);
                    break;
                }
            case 2:
                {
                    GiveArrowPosition(0, 1.33f, 1.02f);
                    GiveArrowPosition(1, -1.27f, 1f);
                    GiveArrowPosition(2, 1.35f, -0.37f);
                    GiveArrowPosition(3, -1.29f, -0.38f);
                    GiveArrowPosition(4, 0.85f, 2.08f);
                    GiveArrowPosition(5, -0.66f, 2.08f);
                    break;
                }
            case 3:
                {
                    GiveArrowPosition(0, 1.6f, 1.6f);
                    GiveArrowPosition(1, -1.6f, 1.6f);
                    GiveArrowPosition(2, 1.6f, -0.2f);
                    GiveArrowPosition(3, -1.6f, -0.2f);
                    GiveArrowPosition(4, 0.85f, 2.81f);
                    GiveArrowPosition(5, -0.66f, 2.81f);
                    break;
                }
        }
    }

    void GiveArrowPosition(int arrow, float positionX, float positionY)
    {
        Vector2 positionA = new Vector2(positionX, positionY);
        if (Arrows[arrow] != null)
        {
            Arrows[arrow].localPosition = positionA;
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
        if (placeAble)
        {
            GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
            foreach (GameObject tempBuilding2 in allBuildings)
            {
                tempBuilding2.GetComponent<CircleCollider2D>().enabled = true;
                tempBuilding2.GetComponent<BuildingMain>().canClick = true;
                GameObject.Find("Main Camera").GetComponent<CameraChanger>().builderOn = false;
            }
            grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
            for (int i = 0; i < buildingToPlace.GetComponent<BuildingMain>().size.x; i++)
            {
                for (int y = 0; y < buildingToPlace.GetComponent<BuildingMain>().size.y; y++)
                {
                    grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].placeAble = false;
                }
            }

            GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].building = buildingToPlace.GetComponent<BuildingMain>().buildingName;
            MainGameController.ChangeColliders(true);
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
            if (!tempBuilding.GetComponent<BuildingMain>().decoration)
            {
                Destroy(gameObject);
            }
            else if(rePos)
            {
                Destroy(gameObject);
            }
            else
            {
                AutoMove();
            }
        }
    }

    void AutoMove()
    {
        if (activePlaceOnGrid.x == 0)
        {
            Arrows[0].GetComponent<ChangePositionBuilding>().OnMouseDown();
        }
        else if (activePlaceOnGrid.y == 0)
        {
            Arrows[1].GetComponent<ChangePositionBuilding>().OnMouseDown();
        }
        else if (activePlaceOnGrid.x == GameObject.Find("Grid").GetComponent<Grid>().maxGridSize - buildingToPlace.GetComponent<BuildingMain>().size.x)
        {
            Arrows[3].GetComponent<ChangePositionBuilding>().OnMouseDown();
        }
        else if (activePlaceOnGrid.y == GameObject.Find("Grid").GetComponent<Grid>().maxGridSize - buildingToPlace.GetComponent<BuildingMain>().size.y)
        {
            Arrows[2].GetComponent<ChangePositionBuilding>().OnMouseDown();
        }
        else
        {
            Arrows[0].GetComponent<ChangePositionBuilding>().OnMouseDown();
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
        MainGameController.ChangeColliders(true);
        GameObject.Find("Main Camera").GetComponent<CameraChanger>().builderOn = false;
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
