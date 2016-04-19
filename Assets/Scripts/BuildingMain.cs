using UnityEngine;
using System.Collections;

public class BuildingMain : MonoBehaviour 
{
    public string buildingName;
    public Vector2 size;
    public int price, levelNeeded;
    public bool clicked, busy, buildingBusy, clickedUpgrade;
    Texture2D background, emptyBar, fullBar, taskDone;
    public int level, ID, taskDoing;
    [SerializeField]
    int[] timesForTasks, timesForBuilding;
    public Vector2 gridPosition;
    [SerializeField]
    BuildingPlacer buildingPlacer;
    Account account;
    public float timeToFinishTask, timeToFinishTaskTotal, timeToFinishBuildTotal, timeLeftToFinishBuild;
    public bool building = false, doneWithTask = false;
    bool waitOneSec = false, waitOneSecForBuilding = false;
    GUIStyle smallFont;
    int[,] taskRewards = new int[2, 5] // money - RP
        { {10,20,30,40,50 }, {10,20,30,40,50 } };

    int[,] priceForUpgrading = new int[5, 3] // level, money, RP
        { { 1, 100, 0}, // level 1
        { 3 , 300 , 0}, // level 2
        { 5, 500, 10}, // level 3
        { 8, 1000, 50}, // level 4
        { 10 , 2000, 100 }}; // level 5

    public void GiveInformation(int[,] information)
    {
        priceForUpgrading = information;
    }

    public virtual void Start () 
	{
        smallFont = new GUIStyle();
        background = GameObject.Find("HUD").GetComponent<HUD>().background;
        emptyBar = GameObject.Find("HUD").GetComponent<HUD>().emptyBar;
        fullBar = GameObject.Find("HUD").GetComponent<HUD>().fullBar;
        taskDone = GameObject.Find("HUD").GetComponent<HUD>().taskDone;
        account = GameObject.Find("Account").GetComponent<Account>();
    }

    public virtual void Update()
    {
        if(building)
        {
            if(timeLeftToFinishBuild <= 0)
            {
                building = false;
                level++;
                account.PushSave();
            }
            else
            {
                if (!waitOneSecForBuilding)
                {
                    StartCoroutine(WaitForBuild());
                }
            }
        }
        if (busy)
        {
            if (timeToFinishTask > 0)
            {
                busy = true;
                if (!waitOneSec)
                {
                    StartCoroutine(WaitForTask());
                }
            }
            else
            {
                busy = false;
                doneWithTask = true;
            }
        }
    }

    IEnumerator WaitForTask()
    {
        waitOneSec = true;
        timeToFinishTask--;
        yield return new WaitForSeconds(1);
        waitOneSec = false;
    }

    IEnumerator WaitForBuild()
    {
        waitOneSecForBuilding = true;
        timeLeftToFinishBuild--;
        yield return new WaitForSeconds(1);
        waitOneSecForBuilding = false;
    }

    void OnMouseDown()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject buildingTemp in buildings)
        {
            //Debug.Log(buildingTemp.name);
            buildingTemp.GetComponent<BuildingMain>().clicked = false;
        }
        clicked = true;
        if(doneWithTask)
        {
            GetReward();
            clicked = false;
        }
    }

    void GetReward()
    {
        doneWithTask = false;
        account.money += taskRewards[0, taskDoing];
        account.researchPoints += taskRewards[1, taskDoing];
    }

    public void SetMaxTime()
    {
        timeToFinishBuildTotal = timesForBuilding[level];
    }

    void OnGUI()
    {
        if (clicked)
        {
            GUI.DrawTexture(new Rect(Screen.width / 3, Screen.height / 4, Screen.width / 3, Screen.height / 2), background);
            GUI.Label(new Rect(Screen.width / 3 + 60, Screen.height / 4, Screen.width / 3, Screen.height / 2), buildingName);
            GUI.Label(new Rect(Screen.width / 3 + 60, Screen.height / 2, Screen.width / 3, Screen.height / 2), (level + 1).ToString());
            if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 4, 50, 50), "Back"))
            {
                clicked = false;
            }
            if (GUI.Button(new Rect(Screen.width / 3 + 50, Screen.height / 4, 100, 50), "Upgrade"))
            {
                clickedUpgrade = true;
                clicked = false;
            }
            for (int i = 0; i < timesForTasks.Length; i++)
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 3 + (i * 50), 150, 50), (i + 1) + " Task " + timesForTasks[i] + " Sec"))
                {
                    timeToFinishTaskTotal = timesForTasks[i];
                    timeToFinishTask = timeToFinishTaskTotal;
                    account.PushSave();
                    clicked = false;
                    busy = true;
                }
            }
            if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 2, 50, 50), "Pos"))
            {
                Vector3 positionOfNewBuilding = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                BuildingPlacer tempBuilding = (BuildingPlacer)Instantiate(buildingPlacer, positionOfNewBuilding, transform.rotation);
                tempBuilding.buildingToPlace = this.gameObject;
                tempBuilding.activePlaceOnGrid = gridPosition;
                tempBuilding.builderPlacerTemp = buildingPlacer;
                tempBuilding.fieldID = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)gridPosition.x, (int)gridPosition.y].ID;
                tempBuilding.oldBuilding = this.gameObject;
                tempBuilding.rePos = true;
                clicked = false;
                account.autoSave = false;
                tempBuilding.Delete();
            }
        }
        if(clickedUpgrade)
        {
            GUI.DrawTexture(new Rect(Screen.width / 3, Screen.height / 4, Screen.width / 3, Screen.height / 2), background);
            GUI.Label(new Rect(Screen.width / 3 + 60, Screen.height / 2, Screen.width / 3, Screen.height / 2), "Level Needed: " + priceForUpgrading[level, 0]);
            GUI.Label(new Rect(Screen.width / 3 + 60, Screen.height / 2 + 20, Screen.width / 3, Screen.height / 2), "Money Needed: " + priceForUpgrading[level, 1]);
            GUI.Label(new Rect(Screen.width / 3 + 60, Screen.height / 2 + 40, Screen.width / 3, Screen.height / 2), "Research Needed: " + priceForUpgrading[level, 2]);
            if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 4, 50, 50), "Back"))
            {
                clickedUpgrade = false;
                clicked = true;
            }
            if (GUI.Button(new Rect(Screen.width / 3 + 50, Screen.height / 4, 100, 50), "Upgrade") && CheckIfEnoughResources())
            {
                building = true;
                timeLeftToFinishBuild = timesForBuilding[level];
                SetMaxTime();
                clickedUpgrade = false;
            }
        }
        if (busy)
        {
            ShowBar(timeToFinishTaskTotal, timeToFinishTask);
        }
        if(building)
        {
            ShowBar(timeToFinishBuildTotal, timeLeftToFinishBuild);
        }
        if (doneWithTask)
        {
            Vector3 tempPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, -transform.position.y));
            if(GUI.Button(new Rect(tempPos.x - 23, tempPos.y - 78, 46, 30), taskDone, smallFont))
            {
                GetReward();
            }
        }
    }

    bool CheckIfEnoughResources()
    {
        bool enough = true;
        if(priceForUpgrading[level, 0] > account.level)
        {
            enough = false;
        }
        if (priceForUpgrading[level, 1] > account.money)
        {
            enough = false;
        }
        if (priceForUpgrading[level, 2] > account.researchPoints)
        {
            enough = false;
        }
        if(enough)
        {
            account.money -= priceForUpgrading[level, 1];
            account.researchPoints -= priceForUpgrading[level, 2];
        }
        return enough;
    }

    void ShowBar(float max, float min)
    {
        Vector3 tempPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, -transform.position.y));
        GUI.Label(new Rect(tempPos.x - 25, tempPos.y - 105, 50, 25), min.ToString() + " Sec");
        GUI.DrawTexture(new Rect(tempPos.x - 25, tempPos.y - 80, 50, 15), emptyBar);
        float perc = max / min;
        GUI.DrawTexture(new Rect(tempPos.x - 23, tempPos.y - 78, 46 / perc, 11), fullBar);
    }
}
