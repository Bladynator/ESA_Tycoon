using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingMain : MonoBehaviour 
{
    public string buildingName;
    public Vector2 size;
    public int price, rpPrice;
    public int[] levelsNeeded;
    [SerializeField]
    int[] timesForTasks, timesForBuilding;
    [SerializeField]
    int[] taskRewards = new int[4] // money - RP
        {50,20,30,40 };
    [SerializeField]
    bool resourceBuilding = true, decoration = false;
    [SerializeField]
    string minigame = "";

    [HideInInspector]
    public bool busy, buildingBusy, clickedUpgrade;
    [HideInInspector]
    public Vector2 gridPosition;

    [Header("Don't Change")]
    [SerializeField]
    BuildingPlacer buildingPlacer;
    public int level, ID, taskDoing = -1;
    Account account;
    public float timeToFinishTask, timeToFinishTaskTotal, timeToFinishBuildTotal, timeLeftToFinishBuild;
    public bool building = false, doneWithTask = false, onceToCreate = false;
    bool waitOneSec = false, waitOneSecForBuilding = false;
    

    int[,] priceForUpgrading = new int[5, 3]; // level, money, RP

    GameObject[] canvas;
    GameObject tempBar;
    Button[] allButtons;

    public bool ableToSave = true;
    
    public virtual void Start()
    {
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
        canvas = GameObject.Find("HUD").GetComponent<HUD>().buildingCanvas;
        if(!resourceBuilding)
        {
            taskRewards = new int[4];
        }
    }

    public virtual void Update()
    {
        SortingLayers();

        #region BuildingTimer
        if (building)
        {
            if (timeLeftToFinishBuild <= 0)
            {
                building = false;
                level++;
                Destroy(tempBar);
                if(ableToSave)
                {
                    account.PushSave();
                }
            }
            else
            {
                if (!waitOneSecForBuilding)
                {
                    StartCoroutine(WaitForBuild());
                }
            }
        }
        #endregion
        #region TaskTimer
        if (busy)
        {
            if (timeToFinishTask <= 0 && !onceToCreate)
            {
                busy = false;
                doneWithTask = true;
                onceToCreate = true;
                GetComponent<CircleCollider2D>().enabled = false;
                Destroy(tempBar);
                StopCoroutine(WaitForTask());
                Destroy(tempBar);
                if (!resourceBuilding)
                {
                    GetReward();
                }
                else
                {
                    tempBar = (GameObject)Instantiate(canvas[3], transform.position + new Vector3(0, 3, 0), transform.rotation);
                    tempBar.GetComponentInChildren<Button>().onClick.AddListener(delegate { GetReward(); });
                }
            }
            else
            {
                if (!waitOneSec && !onceToCreate && timeToFinishTask > 0)
                {
                    StartCoroutine(WaitForTask());
                }
            }
        } 
        #endregion
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (!busy && !building && !doneWithTask)
            {
                canvas[0].SetActive(true);
                setupFirstButtons(canvas[0]);
                account.ChangeColliders(false);
                GameObject.Find("HUD").GetComponent<HUD>().EnableButton(false);
            }
        }
    }

    public void GiveInformation(int[,] information)
    {
        priceForUpgrading = information;
    }

    public void SetMaxTime()
    {
        timeToFinishBuildTotal = timesForBuilding[level];
    }

    #region Canvas
    void Busy()
    {
        if(canvas == null)
        {
            canvas = GameObject.Find("HUD").GetComponent<HUD>().buildingCanvas;
        }
        tempBar = (GameObject)Instantiate(canvas[2], transform.position + new Vector3(0, 3, 0), transform.rotation);
    }

    void Building()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("HUD").GetComponent<HUD>().buildingCanvas;
        }
        tempBar = (GameObject)Instantiate(canvas[2], transform.position + new Vector3(0, 3, 0), transform.rotation);
    }

    void DrawBar(float max, float min)
    {
        if (tempBar != null && tempBar != canvas[3])
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

    #region ButtonInnit

    public void setupFirstButtons(GameObject canvasTemp)
    {
        Text[] allText = canvasTemp.GetComponentsInChildren<Text>();
        allText[0].text = buildingName;
        allButtons = canvasTemp.GetComponentsInChildren<Button>();
        allButtons[0].onClick.AddListener(delegate { BackClicked(); });
        allButtons[2].onClick.AddListener(delegate { ReposClicked(); });
        if (!decoration)
        {
            allText[1].text = (level + 1).ToString();
            allText[5].text = "Task 1";
            allText[6].text = "Task 2";
            allText[7].text = "Task 3";
            allText[8].text = "Task 4";
            allText[9].text = "Money: " + priceForUpgrading[level, 1] + "\n" + "RP      : " + priceForUpgrading[level, 2];
            allButtons[1].onClick.AddListener(delegate { UpgradeClickedFinal(); });
            
            if (resourceBuilding)
            {
                allButtons[3].onClick.AddListener(delegate { TaskClicked(0); });
                allButtons[3].GetComponentInChildren<Text>().text = taskRewards[0] + " Coins " + timesForTasks[0] + " Sec.";
                allButtons[4].onClick.AddListener(delegate { TaskClicked(1); });
                allButtons[4].GetComponentInChildren<Text>().text = taskRewards[1] + " Coins " + timesForTasks[1] + " Sec.";
                allButtons[5].onClick.AddListener(delegate { TaskClicked(2); });
                allButtons[5].GetComponentInChildren<Text>().text = taskRewards[2] + " Coins " + timesForTasks[2] + " Sec.";
                allButtons[6].onClick.AddListener(delegate { TaskClicked(3); });
                allButtons[6].GetComponentInChildren<Text>().text = taskRewards[3] + " Coins " + timesForTasks[3] + " Sec.";
            }
            else
            {
                allButtons[3].onClick.AddListener(delegate { ClickedMinigame(0, minigame); });
                allButtons[3].GetComponentInChildren<Text>().text = "Easy";
                allButtons[4].onClick.AddListener(delegate { ClickedMinigame(1, minigame); });
                allButtons[4].GetComponentInChildren<Text>().text = "Medium";
                allButtons[5].onClick.AddListener(delegate { ClickedMinigame(2, minigame); });
                allButtons[5].GetComponentInChildren<Text>().text = "Hard";
                allButtons[6].onClick.AddListener(delegate { ClickedMinigame(3, minigame); });
                allButtons[6].GetComponentInChildren<Text>().text = "Endless";
            }
            for (int i = 0; i < 4; i++)
            {
                if (level < i)
                {
                    allButtons[i + 3].enabled = false;
                }
            }
        }
        else
        {
            allButtons[1].onClick.AddListener(delegate { DeleteDeco(); });
            for (int i = 3; i < allButtons.Length; i++)
            {
                allButtons[i].gameObject.SetActive(false);
            }
            Destroy(allText[1]);
            allText[300].text = "Destroy";
            for (int i = 5; i < allText.Length; i++)
            {
                Destroy(allText[i]);
            }
        }
    }
    #endregion

    #region Buttons

    void DeleteDeco()
    {
        Vector3 positionOfNewBuilding = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        BuildingPlacer tempBuilding = (BuildingPlacer)Instantiate(buildingPlacer, positionOfNewBuilding, transform.rotation);
        tempBuilding.buildingToPlace = this.gameObject;
        tempBuilding.activePlaceOnGrid = gridPosition;
        tempBuilding.builderPlacerTemp = buildingPlacer;
        tempBuilding.fieldID = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)gridPosition.x, (int)gridPosition.y].ID;
        tempBuilding.oldBuilding = this.gameObject;
        tempBuilding.rePos = true;
        tempBuilding.oldActivePlace = gridPosition;
        BackClicked();
        tempBuilding.Delete();
        GameObject.FindGameObjectWithTag("Builder").GetComponent<BuildingPlacer>().Delete();
        GameObject.Find("Account").GetComponent<Account>().ChangeColliders(true);
        Destroy(GameObject.FindGameObjectWithTag("Builder"));
        Destroy(gameObject);
        account.PushSave();
    }

    void ClickedMinigame(int minigameDifficulty, string minigame)
    {
        TaskClicked(minigameDifficulty);
        if (ableToSave)
        {
            account.PushSave();
        }
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().ActivateMiniGame(minigame, minigameDifficulty);
    }

    public void UpgradeClickedFinal()
    {
        if (CheckIfEnoughResources())
        {
            Building();
            building = true;
            timeLeftToFinishBuild = timesForBuilding[level];
            SetMaxTime();
            BackClicked();
        }
    }

    public void TaskClicked(int task)
    {
        Busy();
        timeToFinishTaskTotal = timesForTasks[task];
        timeToFinishTask = timeToFinishTaskTotal;
        taskDoing = task;
        if (ableToSave)
        {
            account.PushSave();
        }
        BackClicked();
        busy = true;
    }

    public void BackClicked()
    {
        canvas[0].SetActive(false);
        account.ChangeColliders(true);
        GameObject.Find("HUD").GetComponent<HUD>().EnableButton();
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
        tempBuilding.oldActivePlace = gridPosition;
        BackClicked();
        account.autoSave = false;
        tempBuilding.Delete();
    }
    #endregion 
    #endregion

    void SortingLayers()
    {
        int layerSort = Mathf.RoundToInt(((gridPosition.x + size.x) + (gridPosition.y + size.y)) - (size.x / 2 + size.y / 2));
        layerSort *= -layerSort;
        GetComponent<SpriteRenderer>().sortingOrder = layerSort;
    }

    #region Timers
    IEnumerator WaitForTask()
    {
        waitOneSec = true;
        timeToFinishTask--;
        DrawBar(timeToFinishTaskTotal, timeToFinishTask);
        yield return new WaitForSeconds(1);
        waitOneSec = false;
    }

    IEnumerator WaitForBuild()
    {
        waitOneSecForBuilding = true;
        timeLeftToFinishBuild--;
        yield return new WaitForSeconds(1);
        DrawBar(timeToFinishBuildTotal, timeLeftToFinishBuild);
        waitOneSecForBuilding = false;
    } 
    #endregion
    
    public void GetReward()
    {
        doneWithTask = false;
        account.money += taskRewards[0];
        taskDoing = -1;
        Destroy(tempBar);
        onceToCreate = false;
        GetComponent<CircleCollider2D>().enabled = true;
        if (ableToSave)
        {
            account.PushSave();
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
}
