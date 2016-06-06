using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingMain : MonoBehaviour 
{
    public string buildingName;
    public Vector2 size;
    public int[] levelsNeededNewBuilding;
    public int[] levelsNeededUpgrade;
    public int[] moneyNeededUpgrade;
    public int[] rpNeededUpgrade;
    [SerializeField]
    int[] timesForTasks, timesForBuilding;
    [SerializeField]
    string[] taskNames = new string[4];
    [SerializeField]
    int[] taskRewards = new int[4] // money - RP
        {50,20,30,40 };
    [SerializeField]
    public bool resourceBuilding = true, decoration = false;
    [SerializeField]
    string minigame = "", buildingInformation;
    public Sprite[] buildingSprites;
    public int[] exp;

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

    float currentTime = 0;
    string[] minigameDiff = new string[4] {"Easy","Medium","Hard","Endless" };

    int[,] priceForUpgrading = new int[4, 3]
        { { 1, 100, 0}, // level 1
        { 4 , 300 , 0}, // level 2
        { 6, 500, 10}, // level 3
        { 8, 1000, 50}}; // level 4; // level, money, RP

    GameObject[] canvas;
    GameObject tempBar;
    Button[] allButtons;
    Image[] allImages;
    Text[] allText;

    public bool ableToSave = true;
    
    public virtual void Start()
    {
        if (!decoration)
        {
            for (int i = 0; i < 4; i++)
            {
                priceForUpgrading[i, 0] = levelsNeededUpgrade[i];
                priceForUpgrading[i, 1] = moneyNeededUpgrade[i];
                priceForUpgrading[i, 2] = rpNeededUpgrade[i];
            }
        }
        if(size.x != size.y)
        {
            float newSize;
            if (size.x < size.y)
            {
                newSize = size.x;
            }
            else
            {
                newSize = size.y;
            }
            transform.localScale = new Vector3(newSize, newSize, 1);
        }
        else if(size.x == 2 && size.y == 2)
        {
            transform.localScale = new Vector2(3 * 0.98f, 3 * 0.95f);
        }
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
        if (buildingSprites[level] != null)
        {
            GetComponent<SpriteRenderer>().sprite = buildingSprites[level];
        }
    }

    public virtual void Update()
    {
        SortingLayers();

        if(Time.time >= currentTime + 1f)
        {
            currentTime = Time.time;
            if (busy)
            {
                timeToFinishTask--;
                DrawBar(timeToFinishTaskTotal, timeToFinishTask);
                if (timeToFinishTask <= 0 && !onceToCreate)
                {
                    busy = false;
                    doneWithTask = true;
                    onceToCreate = true;
                    GetComponent<CircleCollider2D>().enabled = false;
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

            }
            if(building)
            {
                timeLeftToFinishBuild--;
                DrawBar(timeToFinishBuildTotal, timeLeftToFinishBuild);
                if (timeLeftToFinishBuild <= 0)
                {
                    building = false;
                    level++;
                    Destroy(tempBar);
                    account.exp += exp[level];
                    Instantiate(GameObject.Find("HUD").GetComponent<HUD>().particleUpgrade, transform.position, transform.rotation);
                    if (buildingSprites[level] != null)
                    {
                        GetComponent<SpriteRenderer>().sprite = buildingSprites[level];
                    }
                    if (ableToSave)
                    {
                        account.PushSave();
                    }
                }
            }
        }
    }

    public void OnMouseUp()
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
        if (!resourceBuilding)
        {
            tempBar = (GameObject)Instantiate(canvas[2], transform.position + new Vector3(0, 3, 0), transform.rotation);
        }
        else
        {
            tempBar = (GameObject)Instantiate(canvas[4], transform.position + new Vector3(0, 3, 0), transform.rotation);
        }
    }

    void Building()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("HUD").GetComponent<HUD>().buildingCanvas;
        }
        tempBar = (GameObject)Instantiate(canvas[5], transform.position + new Vector3(0, 3, 0), transform.rotation);
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

            if(min < 60)
            {
                tempBar.GetComponentInChildren<Text>().text = min + " s";
            }
            else if(min < 3600)
            {
                tempBar.GetComponentInChildren<Text>().text = min / 60 + " m";
            }
            else if(min < 216000)
            {
                tempBar.GetComponentInChildren<Text>().text = min / 60 / 60 + " h";
            }
        }
    }

    #region ButtonInnit

    public void setupFirstButtons(GameObject canvasTemp)
    {
        allButtons = canvasTemp.GetComponentsInChildren<Button>();
        allImages = canvasTemp.GetComponentsInChildren<Image>();
        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[i].onClick.RemoveAllListeners();
        }
        allText = canvasTemp.GetComponentsInChildren<Text>();
        allText[0].text = buildingName + " - LVL " + (level + 1).ToString();
        allImages[16].sprite = buildingSprites[level];
        allButtons[0].onClick.AddListener(delegate { BackClicked(); });
        GameObject.Find("BackButton2").GetComponent<Button>().onClick.AddListener(delegate { BackClicked(); });
        allButtons[2].onClick.AddListener(delegate { ReposClicked(); });
        GameObject.Find("FullBarLevelBuilding").GetComponent<Image>().fillAmount = (float)(level + 1) / 4;

        ChangeResourceIconsTasks(true);

        if (!decoration)
        {
            allText[2].text = "Upgrade";
            allText[1].text = "";
            /*
            allText[4].text = "Task 1";
            allText[5].text = "Task 2";
            allText[6].text = "Task 3";
            allText[7].text = "Task 4";
            */
            if (level != 3)
            {
                allText[3].text = priceForUpgrading[level + 1, 1] + "\n" + priceForUpgrading[level + 1, 2] + "\n" + priceForUpgrading[level + 1, 0];
                allButtons[1].interactable = true;
                allButtons[1].onClick.AddListener(delegate { UpgradeClickedFinal(); });
                if (!CheckIfEnoughResources())
                {
                    allButtons[1].interactable = false;
                }
            }
            else
            {
                allText[3].text = "Max Level";
            }
            
            if (resourceBuilding)
            {
                allText[4].text = "Tasks";
                for (int i = 0; i < 4; i++)
                {
                    int tempi = i;
                    allButtons[tempi + 3].onClick.AddListener(delegate { TaskClicked(tempi); });
                    Text[] allText2 = allButtons[tempi + 3].GetComponentsInChildren<Text>();
                    allText2[0].text = taskNames[tempi];
                    allText2[1].text = taskRewards[tempi].ToString();
                    allText2[2].text = timesForTasks[tempi].ToString();
                    allText2[3].text = "";
                }
            }
            else
            {
                allText[4].text = "Mini Game\nDifficulty";
                ChangeResourceIconsTasks(false);
                for (int i = 0; i < 4; i++)
                {
                    int tempi = i;
                    allButtons[tempi + 3].onClick.AddListener(delegate { ClickedMinigame(tempi, minigame); });
                    Text[] allText2 = allButtons[tempi + 3].GetComponentsInChildren<Text>();
                    allText2[0].text = "";
                    allText2[1].text = "";
                    allText2[2].text = "";
                    allText2[3].text = minigameDiff[tempi];
                }
            }
            for(int i = 3; i < 7; i++)
            {
                allButtons[i].interactable = true;
            }

            if(level < 3)
            {
                allButtons[6].interactable = false;
            }
            if(level < 2)
            {
                allButtons[5].interactable = false;
            }
            if(level < 1)
            {
                allButtons[4].interactable = false;
            }

            if(!resourceBuilding && account.level < 2)
            {
                allButtons[3].interactable = false;
            }
        }
        else
        {
            allButtons[1].onClick.AddListener(delegate { DeleteDeco(); });
            for (int i = 3; i < allButtons.Length; i++)
            {
                allButtons[i].gameObject.SetActive(false);
            }
            allText[1].gameObject.SetActive(false);
            allText[2].text = "Destroy";
            
            for (int i = 5; i < allText.Length; i++)
            {
                allText[i].gameObject.SetActive(false);
            }
            for (int i = 9; i < allImages.Length; i++)
            {
                if (i != 16)
                {
                    allImages[i].gameObject.SetActive(false);
                }
            }

        }

        allButtons[allButtons.Length - 1].onClick.AddListener(delegate { GiveBuildingInformation(buildingInformation); });

    }
    #endregion

    #region Buttons

    void GiveBuildingInformation(string info)
    {
        GameObject.Find("TextInformationBuilding").GetComponent<Text>().text = info;
    }

    void ChangeResourceIconsTasks(bool toChange)
    {
        allImages[5].enabled = toChange;
        allImages[6].enabled = toChange;

        allImages[8].enabled = toChange;
        allImages[9].enabled = toChange;

        allImages[11].enabled = toChange;
        allImages[12].enabled = toChange;

        allImages[14].enabled = toChange;
        allImages[15].enabled = toChange;
    }

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
        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[i].gameObject.SetActive(true);
        }
        allText[1].gameObject.SetActive(true);
        for (int i = 5; i < allText.Length; i++)
        {
            allText[i].gameObject.SetActive(true);
        }
        for (int i = 9; i < allImages.Length; i++)
        {
            allImages[i].gameObject.SetActive(true);
        }
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
    
    public void GetReward()
    {
        doneWithTask = false;
        account.money += taskRewards[taskDoing];
        taskDoing = -1;
        Destroy(tempBar);
        onceToCreate = false;
        GetComponent<CircleCollider2D>().enabled = true;
        Instantiate(GameObject.Find("HUD").GetComponent<HUD>().particleReward, transform.position, transform.rotation);
        if (ableToSave)
        {
            account.PushSave();
        }
    }

    bool CheckIfEnoughResources()
    {
        bool enough = true;
        if (priceForUpgrading[level + 1, 0] > account.level)
        {
            enough = false;
        }
        if (priceForUpgrading[level + 1, 1] > account.money)
        {
            enough = false;
        }
        if (priceForUpgrading[level + 1, 2] > account.researchPoints)
        {
            enough = false;
        }

        if (enough)
        {
            account.money -= priceForUpgrading[level + 1, 1];
            account.researchPoints -= priceForUpgrading[level + 1, 2];
        }
        return enough;
    }
}
