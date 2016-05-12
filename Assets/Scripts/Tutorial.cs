using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour 
{
    Quests questLine;
    Dialogs dialogs;
    HUD hud;
    Account account;
    int activeQuest;
    int questPart = 1, oldQuestPart;
    string townName = "ESA ESTEC";
    [SerializeField]
    GameObject hq;
    public bool tutorialDoing = false;
    BuilderMenu builderMenu;
    [SerializeField]
    GameObject arrow, canvas, nameInputCanvas;
    [SerializeField]
    GameObject[] arrowLocations;
    GameObject tempArrow, tempCanvas, tempNameInputCanvas;
    bool arrowSpawned = false, once = false;

    void Start () 
	{
        questLine = GameObject.Find("Quests").GetComponent<Quests>();
        dialogs = GameObject.Find("Quests").GetComponent<Dialogs>();
        hud = GameObject.Find("HUD").GetComponent<HUD>();
        account = GameObject.Find("Account").GetComponent<Account>();
        builderMenu = hud.buildMenu;
    }
	
	void FixedUpdate () 
	{
        activeQuest = questLine.questLineProgress[0];
    }

    void Update()
    {
        switch (activeQuest)
        {
            case 1:
                {
                    switch (questPart)
                    {
                        case 1:
                            {
                                tutorialDoing = true;
                                account.ChangeColliders(false);
                                account.autoSave = false;
                                hud.EnableButton(false);
                                hud.CanvasEnabled(false);
                                questLine.ShowQuests(false);
                                dialogs.tutorial = true;
                                ShowDialog(0);
                                break;
                            }
                        case 2:
                            {
                                MakeCanvasName(townName);
                                break;
                            }
                        case 3:
                            {
                                ShowDialog(1);
                                break;
                            }
                        case 4:
                            {
                                MakeCanvas("You got: 1000 Gold!", 0, 0, 0);
                                break;
                            }
                        case 5:
                            {
                                ShowDialog(2);
                                break;
                            }
                        case 6:
                            {
                                PlaceHQ(hq);
                                questPart++;
                                break;
                            }
                        case 7:
                            {
                                ShowDialog(3);
                                break;
                            }
                        case 8:
                            {
                                ShowArrow(0);
                                questLine.ShowQuests(true);
                                if (questLine.questOpen != -1)
                                {
                                    questLine.tutorialBack = true;
                                    questPart++;
                                    DestroyArrow();
                                }
                                break;
                            }
                        case 9:
                            {
                                ShowArrow(1);
                                if (questLine.tutorialBack == false)
                                {
                                    questPart++;
                                    DestroyArrow();
                                }
                                break;
                            }
                        case 10:
                            {
                                ShowDialog(4);
                                break;
                            }
                        case 11:
                            {
                                ShowArrow(4);
                                account.ChangeColliders(true);
                                questPart++;
                                break;
                            }
                        case 12:
                            {
                                questPart = -2;
                                break;
                            }
                        case -1:
                            {
                                if (!dialogs.talk)
                                {
                                    questPart = ++oldQuestPart;
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    break;
                }
            case 2:
                {
                    switch(questPart)
                    {
                        case 1:
                            {
                                ShowDialog(5, false);
                                break;
                            }
                        case 2:
                            {
                                MakeCanvas("You got 10 Research Points!", 0, 10, 1);
                                break;
                            }
                        case 3:
                            {
                                ShowDialog(6, false);
                                questLine.tutorialBack = true;
                                ShowArrow(0);
                                break;
                            }
                        case 4:
                            {
                                if (questLine.questOpen != -1)
                                {
                                    questLine.tutorialBack = true;
                                    questPart = 20;
                                    DestroyArrow();
                                }
                                break;
                            }
                        case 5:
                            {
                                ShowArrow(4);
                                if (!builderMenu.tutorialBack)
                                {
                                    DestroyArrow();
                                    questPart++;
                                    builderMenu.tutorialBack = true;
                                }
                                break;
                            }
                        case 6:
                            {
                                ShowArrow(3);
                                if (!builderMenu.tutorialBack)
                                {
                                    DestroyArrow();
                                    questPart++;
                                    builderMenu.tutorialBack = true;
                                }
                                break;
                            }
                        case 7:
                            {
                                questPart = -3;
                                break;
                            }
                        case 20:
                            {
                                ShowArrow(1);
                                if (questLine.tutorialBack == false)
                                {
                                    questLine.tutorialBack = true;
                                    DestroyArrow();
                                    questPart = 5;
                                    builderMenu.tutorialBack = true;
                                }
                                break;
                            }
                        case -1:
                            {
                                if (!dialogs.talk)
                                {
                                    questPart = ++oldQuestPart;
                                }
                                break;
                            }
                        case -2:
                            {
                                questPart = 1;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    break;
                }
            case 3:
                {
                    switch(questPart)
                    {
                        case 1:
                            {
                                ShowDialog(7, false);
                                break;
                            }
                        case 2:
                            {
                                MakeCanvas("You got: 500 Gold!", 500, 0, 2);
                                break;
                            }
                        case 3:
                            {
                                ShowDialog(8, false);
                                hud.EnableButton(false);
                                questLine.ShowQuests(false);
                                GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
                                foreach(GameObject temp in allBuildings)
                                {
                                    temp.GetComponent<BuildingMain>().ableToSave = false;
                                }
                                break;
                            }
                        case 4:
                            {
                                if(account.money == 1250)
                                {
                                    questPart++;
                                }
                                break;
                            }
                        case 5:
                            {
                                ShowDialog(9, false);
                                break;
                            }
                        case 6:
                            {
                                ShowArrow(0);
                                questLine.ShowQuests(true);
                                hud.EnableButton(true);
                                if (questLine.questOpen != -1)
                                {
                                    questLine.tutorialBack = true;
                                    questPart++;
                                    DestroyArrow();
                                }
                                break;
                            }
                        case 7:
                            {
                                ShowArrow(1);
                                if (questLine.tutorialBack == false)
                                {
                                    questPart = -6;
                                    DestroyArrow();
                                }
                                break;
                            }
                        
                        case -1:
                            {
                                if (!dialogs.talk)
                                {
                                    questPart = ++oldQuestPart;
                                }
                                break;
                            }
                        case -3:
                            {
                                questPart = 1;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    break;
                }
            case 0:
                {
                    switch (questPart)
                    {
                        case -6:
                            {
                                if (questLine.questLineProgress[0] == 0)
                                {
                                    questPart = 7;
                                }
                                break;
                            }
                        case 7:
                            {
                                hud.EnableButton(true);
                                questLine.ShowQuests(true);
                                ShowDialog(10, false);
                                break;
                            }
                        case 8:
                            {
                                tutorialDoing = false;
                                questPart++;
                                GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
                                foreach (GameObject temp in allBuildings)
                                {
                                    temp.GetComponent<BuildingMain>().ableToSave = true;
                                }
                                Destroy(gameObject);
                                questLine.questLineProgress[1] = 1;
                                questLine.ResetQuests();
                                account.autoSave = true;
                                account.PushSave();
                                break;
                            }
                        case -1:
                            {
                                if (!dialogs.talk)
                                {
                                    questPart = ++oldQuestPart;
                                }
                                break;
                            }
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    void PressedDoneButton(string name)
    {
        hud.SetName(name);
        questPart++;
        Destroy(tempNameInputCanvas);
    }

    void MakeCanvasName(string name)
    {
        if (tempNameInputCanvas == null)
        {
            tempNameInputCanvas = Instantiate(nameInputCanvas);
            tempNameInputCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedDoneButton(name); });
            tempNameInputCanvas.GetComponentInChildren<InputField>().text = name;
            tempNameInputCanvas.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate { tempNameInputCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedDoneButton(tempNameInputCanvas.GetComponentInChildren<InputField>().text); }); });
        }
    }

    void PressedCollectButton(int addedMoney, int addedRP, int time)
    {
        account.money += addedMoney;
        account.researchPoints += addedRP;

        questPart++;
        if (time != 2)
        {
            hud.canvas[time].SetActive(true);
        }

        Destroy(tempCanvas);
    }

    void MakeCanvas(string text, int addedMoney, int addedRP, int time)
    {
        if(tempCanvas == null)
        {
            tempCanvas = Instantiate(canvas);
            tempCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedCollectButton(addedMoney, addedRP, time); });
            tempCanvas.GetComponentInChildren<Text>().text = text;
        }
    }

    void ShowArrow(int location)
    {
        if (!arrowSpawned)
        {
            arrowSpawned = true;
            tempArrow = (GameObject)Instantiate(arrow, arrowLocations[location].transform.position, arrowLocations[location].transform.rotation);
        }
        tempArrow.transform.position = arrowLocations[location].transform.position;
    }

    public void DestroyArrow()
    {
        Destroy(tempArrow);
        arrowSpawned = false;
    }

    void ShowDialog(int part, bool tutorial = true)
    {
        dialogs.tutorial = tutorial;
        dialogs.ActivateTalking(part);
        oldQuestPart = questPart;
        questPart = -1;
    }

    void PlaceHQ(GameObject hq)
    {
        Vector2 activePlaceOnGrid = new Vector2(5, 5);
        EmptyField[,] grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
        for (int i = 0; i < hq.GetComponent<BuildingMain>().size.x; i++)
        {
            for (int y = 0; y < hq.GetComponent<BuildingMain>().size.y; y++)
            {
                grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].placeAble = false;
            }
        }

        GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].building = hq.GetComponent<BuildingMain>().buildingName;
        Vector2 size = hq.GetComponent<BuildingMain>().size;
        Vector3 newPosition = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].transform.position;
        GameObject tempBuilding = (GameObject)Instantiate(hq, newPosition, transform.rotation);

        tempBuilding.transform.localScale = size;
        tempBuilding.GetComponent<BuildingMain>().ID = GameObject.Find("Grid").GetComponent<Grid>().grid[(int)activePlaceOnGrid.x, (int)activePlaceOnGrid.y].ID;
        tempBuilding.GetComponent<BuildingMain>().gridPosition = activePlaceOnGrid;
    }
}
