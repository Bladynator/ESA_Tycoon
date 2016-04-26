using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingMain : MonoBehaviour 
{
    public string buildingName;
    public Vector2 size;
    public int price, levelNeeded;
    [SerializeField]
    int[] timesForTasks, timesForBuilding;
    public GUIStyle buildingText;

    [HideInInspector]
    public bool busy, buildingBusy, clickedUpgrade;
    [HideInInspector]
    public Vector2 gridPosition;

    [Header("Don't Change")]
    [SerializeField]
    BuildingPlacer buildingPlacer;
    public int level, ID, taskDoing;
    Account account;
    public float timeToFinishTask, timeToFinishTaskTotal, timeToFinishBuildTotal, timeLeftToFinishBuild;
    public bool building = false, doneWithTask = false, onceToCreate = false;
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

    public GameObject canvas, upgradeCanvas, buildingExtras, collectButton;
    GameObject tempCanvas, tempBar;
    Button[] allButtons;

    public void GiveInformation(int[,] information)
    {
        priceForUpgrading = information;
    }

    public virtual void Start()
    {
        smallFont = new GUIStyle();
        smallFont.normal.textColor = Color.black;
        account = GameObject.Find("Account").GetComponent<Account>();
        Input.simulateMouseWithTouches = true;
        if (busy)
        {
            Busy();
        }
        if (building)
        {
            Building();
        }
    }

    void Busy()
    {
        tempBar = (GameObject)Instantiate(buildingExtras, transform.position + new Vector3(0, 3, 0), transform.rotation);
    }

    void Building()
    {
        tempBar = (GameObject)Instantiate(buildingExtras, transform.position + new Vector3(0, 3, 0), transform.rotation);
    }

    public void SetupUpgradeCanvas(GameObject canvasTemp)
    {
        Text[] allText = canvasTemp.GetComponentsInChildren<Text>();
        allText[0].text = buildingName;
        allText[1].text = "Level Needed: " + priceForUpgrading[level, 0];
        allText[2].text = "Money Needed: " + priceForUpgrading[level, 1];
        allText[3].text = "Research Needed: " + priceForUpgrading[level, 2];
        allButtons = canvasTemp.GetComponentsInChildren<Button>();
        allButtons[0].onClick.AddListener(delegate { BackClickedFromUpgrade(); });
        allButtons[1].onClick.AddListener(delegate { UpgradeClickedFinal(); });
    }

    public void BackClickedFromUpgrade()
    {
        Destroy(tempCanvas);
        tempCanvas = Instantiate(canvas);
        setupFirstButtons(tempCanvas);
    }

    public void UpgradeClickedFinal()
    {
        if(CheckIfEnoughResources())
        {
            Building();
            building = true;
            timeLeftToFinishBuild = timesForBuilding[level];
            SetMaxTime();
            BackClicked();
        }
    }

    public void setupFirstButtons(GameObject canvasTemp)
    {
        Text[] allText = canvasTemp.GetComponentsInChildren<Text>();
        allText[0].text = buildingName;
        allText[1].text = (level + 1).ToString();
        allText[5].text = "Task 1";
        allText[6].text = "Task 2";
        allText[7].text = "Task 3";
        allText[8].text = "Task 4";
        allButtons = canvasTemp.GetComponentsInChildren<Button>();
        allButtons[0].onClick.AddListener(delegate { BackClicked(); });
        allButtons[1].onClick.AddListener(delegate { UpgradeClicked(); });
        allButtons[2].onClick.AddListener(delegate { ReposClicked(); });
        for (int i = 0; i < level + 1; i++)
        {
            allButtons[i + 3].onClick.AddListener(delegate { TaskClicked(i); });
        }
    }

    public void TaskClicked(int task)
    {
        Busy();
        timeToFinishTaskTotal = timesForTasks[task];
        timeToFinishTask = timeToFinishTaskTotal;
        account.PushSave();
        BackClicked();
        busy = true;
    }

    public void BackClicked()
    {
        Destroy(tempCanvas);
        account.ChangeColliders(true);
    }

    public void UpgradeClicked()
    {
        Destroy(tempCanvas);
        tempCanvas = Instantiate(upgradeCanvas);
        SetupUpgradeCanvas(tempCanvas);
    }

    public void ReposClicked()
    {
        Vector3 positionOfNewBuilding = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        BuildingPlacer tempBuilding = (BuildingPlacer)Instantiate(buildingPlacer, positionOfNewBuilding, transform.rotation);
        tempBuilding.buildingToPlace = this.gameObject;
        tempBuilding.activePlaceOnGrid = gridPosition;
        tempBuilding.builderPlacerTemp = buildingPlacer;
        tempBuilding.fieldID = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)gridPosition.x, (int)gridPosition.y].ID;
        tempBuilding.oldBuilding = this.gameObject;
        tempBuilding.rePos = true;
        BackClicked();
        account.autoSave = false;
        tempBuilding.Delete();
    }

    public virtual void Update()
    {
        SortingLayers();

        if (building)
        {
            if (timeLeftToFinishBuild <= 0)
            {
                building = false;
                level++;
                Destroy(tempBar);
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
            if (timeToFinishTask <= 0 && !onceToCreate)
            {
                busy = false;
                doneWithTask = true;
                onceToCreate = true;
                Destroy(tempBar);
                tempBar = (GameObject)Instantiate(collectButton, transform.position + new Vector3(0, 3, 0), transform.rotation);
                tempBar.GetComponentInChildren<Button>().onClick.AddListener(delegate { GetReward(); });
                account.ChangeColliders(false);
                StopCoroutine(WaitForTask());
            }
            else
            {
                if (!waitOneSec && !onceToCreate && timeToFinishTask > 0)
                {
                    StartCoroutine(WaitForTask());
                }
            }
        }
    }

    void SortingLayers()
    {
        int layerSort = Mathf.RoundToInt(((gridPosition.x + size.x) + (gridPosition.y + size.y)) - (size.x / 2 + size.y / 2));
        layerSort *= -layerSort;
        GetComponent<SpriteRenderer>().sortingOrder = layerSort;
    }

    IEnumerator WaitForTask()
    {
        waitOneSec = true;
        timeToFinishTask--;
        DrawBar(timeToFinishTaskTotal, timeToFinishTask);
        yield return new WaitForSeconds(1);
        waitOneSec = false;
    }

    void DrawBar(float max, float min)
    {
        if (tempBar != null && tempBar != collectButton)
        {
            Image[] all = tempBar.GetComponentsInChildren<Image>();
            foreach (Image temp in all)
            {
                if (temp.gameObject.tag == "Bar")
                {
                    temp.fillAmount = (min / max);
                }
            }
            tempBar.GetComponentInChildren<Text>().text = "Time Left: " + min + " s";
        }
    }

    IEnumerator WaitForBuild()
    {
        waitOneSecForBuilding = true;
        timeLeftToFinishBuild--;
        yield return new WaitForSeconds(1);
        DrawBar(timeToFinishBuildTotal, timeLeftToFinishBuild);
        waitOneSecForBuilding = false;
    }
    
    void OnMouseDown()
    {
        if (!busy && !building && !doneWithTask)
        {
            tempCanvas = Instantiate(canvas);
            setupFirstButtons(tempCanvas);
            account.ChangeColliders(false);
        }
    }

    public void GetReward()
    {
        doneWithTask = false;
        account.money += taskRewards[0, taskDoing];
        account.researchPoints += taskRewards[1, taskDoing];
        Destroy(tempBar);
        onceToCreate = false;
        account.ChangeColliders(true);
        account.PushSave();
    }

    public void SetMaxTime()
    {
        timeToFinishBuildTotal = timesForBuilding[level];
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
}
