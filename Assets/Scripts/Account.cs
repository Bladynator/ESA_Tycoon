using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class Account : MonoBehaviour 
{
    public int level = 1, money = 1000, researchPoints = 0, exp = 0;
    public string nameTown;
    SaveLoad saveLoad;
    
    string save;
    bool waitOneSec = false;
    public bool autoSave = true;
    int saveInSec = 5;
    public int[] expNeededForLevel;
    public int[] amountOfEachBuilding = new int[8] {0,0,0,0,0,0,0,0 }; // 0 = HQ / 1 = 
	
	void Start () 
	{
        saveLoad = GameObject.Find("SaveLoad").GetComponent<SaveLoad>();
        PushLoad();
    }
    
    void Update()
    {
        if(exp >= expNeededForLevel[level])
        {
            exp -= expNeededForLevel[level];
            level++;
        }
        if (saveInSec <= 0 && autoSave)
        {
            saveInSec = 5;
            PushSave();
        }
        if (!waitOneSec)
        {
            StartCoroutine(ToSave());
        }
    }

    public void UpdateAmountOFBuildings()
    {
        amountOfEachBuilding = new int[8] {0,0,0,0,0,0,0,0 };
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject building in allBuildings)
        {
            if (building.GetComponent<BuildingMain>() != null)
            {
                string nameOfBuilding = building.GetComponent<BuildingMain>().buildingName;
                switch (nameOfBuilding)
                {
                    case "Engineer":
                        {
                            amountOfEachBuilding[0]++;
                            break;
                        }
                    case "Builder":
                        {
                            amountOfEachBuilding[1]++;
                            break;
                        }
                    case "Flag":
                        {
                            amountOfEachBuilding[2]++;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }

    IEnumerator ToSave()
    {
        waitOneSec = true;
        saveInSec--;
        yield return new WaitForSeconds(1);
        waitOneSec = false;
    }

    public void PushSave()
    {
        StopCoroutine(ToSave());
        saveInSec = 5;
        string stringToPush = "";
        stringToPush += GetFieldsToString() + "<DB>" + level + "<DB>" + money + "<DB>" + researchPoints + "<DB>" + DateTime.Now.ToString() + "<DB>" + GetQuestLines() + "<DB>" + exp + "<DB>" + nameTown;
        saveLoad.writeStringToFile(stringToPush, "SaveFile");
    }

    string GetQuestLines()
    {
        string questsToString = "";
        int[] quests = GameObject.Find("Quests").GetComponent<Quests>().questLineProgress;
        foreach (int progress in quests)
        {
            questsToString += progress.ToString() + "<e>";
        }
        return questsToString;
    }

    public void PushLoad()
    {
        save = saveLoad.readStringFromFile("SaveFile");
        if(save == null)
        {
            GameObject.Find("Quests").GetComponent<Quests>().questLineProgress[0] = 1;
            GameObject.Find("Grid").GetComponent<Grid>().MakeGrid();
        }
        else
        {
            PlaceBuildings();
            UpdateAmountOFBuildings();
            Destroy(GameObject.Find("Tutorial"));
        }
        GameObject.Find("Quests").GetComponent<Quests>().ResetQuests();
    }

    void PlaceBuildings()
    {
        GameObject[] allBuildingsToPlace = GameObject.Find("BuilderMenu").GetComponent<BuilderMenu>().GetAllBuildings();
        GameObject.Find("Grid").GetComponent<Grid>().MakeGrid();
        string[] allInformation = Regex.Split(save, "<DB>");
        level = Convert.ToInt32(allInformation[1]);
        money = Convert.ToInt32(allInformation[2]);
        researchPoints = Convert.ToInt32(allInformation[3]);
        exp = Convert.ToInt32(allInformation[6]);
        nameTown = allInformation[7];
        GameObject.Find("HUD").GetComponent<HUD>().SetName(nameTown);

        string[] quests = Regex.Split(allInformation[5], "<e>");
        int[] questsInt = new int[quests.Length - 1];
        for (int i = 0; i < quests.Length; i++)
        {
            if (quests[i] != "")
            {
                questsInt[i] = Convert.ToInt32(quests[i]);
            }
        }
        GameObject.Find("Quests").GetComponent<Quests>().questLineProgress = questsInt;

        string[] buildingsToPlace = Regex.Split(allInformation[0], "<r>");
        int numberToPlace = 0;
        foreach (string tempBuilding in buildingsToPlace)
        {
            GameObject thingToPlace;
            string[] informationOneBuilding = Regex.Split(tempBuilding, "<e>");
            for (int i = 0; i < allBuildingsToPlace.Length; i++)
            {
                if (allBuildingsToPlace[i].GetComponent<BuildingMain>().buildingName == informationOneBuilding[0])
                {
                    thingToPlace = allBuildingsToPlace[i];

                    if (informationOneBuilding[0] != "EmptyField")
                    {
                        BuildingMain buildingToPlace = thingToPlace.GetComponent<BuildingMain>();
                        int placeOfGridX = 0;
                        int placeOfGridY = 0;
                        for (int p = 0; p < buildingToPlace.size.x; p++)
                        {
                            for (int y = 0; y < buildingToPlace.size.y; y++)
                            {
                                GameObject[] allFieldsToFind = GameObject.FindGameObjectsWithTag("EmptyField");
                                foreach(GameObject field in allFieldsToFind)
                                {
                                    if(field.GetComponent<EmptyField>().ID == numberToPlace + 1)
                                    {
                                        placeOfGridX = (int)field.GetComponent<EmptyField>().gridPosition.x;
                                        placeOfGridY = (int)field.GetComponent<EmptyField>().gridPosition.y;
                                    }
                                }
                                GameObject.Find("Grid").GetComponent<Grid>().grid[placeOfGridX + y, placeOfGridY + p].placeAble = false;
                                //grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].tag = "Default";
                            }
                        }

                        GameObject.Find("Grid").GetComponent<Grid>().grid[placeOfGridX, placeOfGridY].building = buildingToPlace.buildingName;
                        
                        ChangeColliders(true);
                        Vector2 size = buildingToPlace.size;
                        //Vector2 newPosition = transform.position;
                        BuildingMain tempBuilding2 = (BuildingMain)Instantiate(buildingToPlace, GameObject.Find("Grid").GetComponent<Grid>().grid[placeOfGridX, placeOfGridY].transform.position, transform.rotation);
                        tempBuilding2.ID = GameObject.Find("Grid").GetComponent<Grid>().grid[placeOfGridX, placeOfGridY].ID;
                        tempBuilding2.transform.localScale = size;
                        tempBuilding2.taskDoing = Convert.ToInt32(informationOneBuilding[1]);
                        TimeSpan sec = DateTime.Now.Subtract(Convert.ToDateTime(allInformation[4]));
                        if (Convert.ToInt32(informationOneBuilding[2]) > 0)
                        {
                            tempBuilding2.busy = true;
                            tempBuilding2.timeToFinishTaskTotal = Convert.ToInt32(informationOneBuilding[5]);
                        }
                        if(Convert.ToInt32(informationOneBuilding[1]) != -1)
                        {
                            tempBuilding2.busy = true;
                        }
                        tempBuilding2.timeToFinishTask = Convert.ToInt32(informationOneBuilding[2]) - (int)sec.TotalSeconds;
                        tempBuilding2.level = Convert.ToInt32(informationOneBuilding[3]);
                        if(Convert.ToInt32(informationOneBuilding[4]) > 0)
                        {
                            tempBuilding2.building = true;
                            tempBuilding2.SetMaxTime();
                        }
                        tempBuilding2.timeLeftToFinishBuild = Convert.ToInt32(informationOneBuilding[4]) - (int)sec.TotalSeconds;
                        tempBuilding2.gridPosition.x = Convert.ToInt32(informationOneBuilding[6]);
                        tempBuilding2.gridPosition.y = Convert.ToInt32(informationOneBuilding[7]);
                    }
                }
            }
            numberToPlace++;
        }
    }
    
    public void ChangeColliders(bool toChange)
    {
        GameObject[] allFields = GameObject.FindGameObjectsWithTag("EmptyField");
        foreach (GameObject tempField in allFields)
        {
            tempField.GetComponent<EmptyField>().ChangeColor(Color.white);
            {
                tempField.GetComponent<BoxCollider>().enabled = toChange;
            }
        }
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject tempField in allBuildings)
        {
            if (tempField.GetComponent<CircleCollider2D>() != null)
            {
                tempField.GetComponent<CircleCollider2D>().enabled = toChange;
            }
        }
        //GameObject.Find("HUD").GetComponent<HUD>().enabled = toChange;
        GameObject.Find("Quests").GetComponent<Quests>().enabled = toChange;
        if(toChange)
        {
            GameObject.Find("HUD").GetComponent<HUD>().EnableButton();
        }
    }

    string GetFieldsToString()
    {
        string fields = "";
        EmptyField[,] grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
        GameObject[] tempBuildings = GameObject.FindGameObjectsWithTag("Building");
        for (int i = 0; i < GameObject.Find("Grid").GetComponent<Grid>().maxGridSize; i++)
        {
            for (int p = 0; p < GameObject.Find("Grid").GetComponent<Grid>().maxGridSize; p++)
            {
                BuildingMain tempBuilding = null;
                if(grid[i, p].building != "EmptyField")
                {
                    for (int k = 0; k < tempBuildings.Length; k++)
                    {
                        //Debug.Log(tempBuildings[k].GetComponent<BuildingMain>().ID + " / " + grid[i, p].ID);
                        if(tempBuildings[k].GetComponent<BuildingMain>().ID == grid[i, p].ID)
                        {
                            tempBuilding = tempBuildings[k].GetComponent<BuildingMain>();
                            break;
                        }
                    }
                }
                fields += grid[i, p].building + "<e>";
                if(tempBuilding != null)
                {
                    fields += tempBuilding.taskDoing + "<e>" + tempBuilding.timeToFinishTask + "<e>" + tempBuilding.level + "<e>" + tempBuilding.timeLeftToFinishBuild + "<e>" + tempBuilding.timeToFinishTaskTotal + "<e>" + tempBuilding.gridPosition.x + "<e>" + tempBuilding.gridPosition.y;
                }
                fields += "<r>";
            }
        }
        return fields;
    }
}
