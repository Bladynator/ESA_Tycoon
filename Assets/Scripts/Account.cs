using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using Facebook.Unity.Mobile.IOS;
using Facebook.Unity.Mobile.Android;
using Facebook.Unity.Editor;
using Facebook.Unity;
using UnityEngine.UI;

public class Account : MonoBehaviour 
{
    public int level = 1, money = 0, researchPoints = 0, exp = 0;
    public string nameTown;
    SaveLoad saveLoad;
    [SerializeField]
    BuildingButtons buildings;
    public string[] namesBuildings;
    [SerializeField]
    Toggle[] soundChecks;

    string save;
    bool waitOneSec = false;
    public bool autoSave = true, justLeveld = false, waitForInput = false;
    int saveInSec = 5;
    public int[] expNeededForLevel, newbuildings;
    public int[] amountOfEachBuilding = new int[10] {0,0,0,0,0,0,0,0,0,0 }; // 0 = HQ / 1 = 

    public AudioClip levelUpSound;
    
    void Start () 
	{
        saveLoad = GameObject.Find("SaveLoad").GetComponent<SaveLoad>();
        PushLoad();
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().levelPlayer = level;
    }

    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!string.IsNullOrEmpty(result.PostId))
        {
            Debug.Log(result.PostId);
        }
        else
        {
            Debug.Log("ShareLink success!");
        }
    }

    void Update()
    {
        if (level != expNeededForLevel.Length)
        {
            if (exp >= expNeededForLevel[level])
            {
                LevelUp();
            }
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

    void LevelUp()
    {
        GameObject.Find("SFXController").GetComponent<AudioSource>().PlayOneShot(levelUpSound);
        exp -= expNeededForLevel[level];
        level++;
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().levelPlayer = level;
        GameObject.Find("HUD").GetComponent<HUD>().ChangeBadge();
        GameObject.Find("HUD").GetComponent<HUD>().notificationNumber = newbuildings[level];
        GameObject.Find("HUD").GetComponent<HUD>().UpdateNotification(newbuildings[level]);
    }

    public void Share()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IOSFacebook temp = new IOSFacebook();
            System.Uri temp3 = new System.Uri("http://www.esa.int/var/esa/storage/images/esa_multimedia/images/2015/03/asteroid_collision/15339990-1-eng-GB/Asteroid_collision_node_full_image_2.jpg");
            temp.ShareLink(new System.Uri("http://www.esa.int/aim"), "AIM - Space Challenge", "The Tycoon for AIM!", temp3, callback: ShareCallback);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            AndroidFacebook temp = new AndroidFacebook();
            System.Uri temp3 = new System.Uri("http://www.esa.int/var/esa/storage/images/esa_multimedia/images/2015/03/asteroid_collision/15339990-1-eng-GB/Asteroid_collision_node_full_image_2.jpg");
            temp.ShareLink(new System.Uri("http://www.esa.int/aim"), "AIM - Space Challenge", "The Tycoon for AIM!", temp3, callback: ShareCallback);
        }
    }

    public void UpdateAmountOFBuildings()
    {
        amountOfEachBuilding = new int[10] {0,0,0,0,0,0,0,0,0,0 };
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        for(int i = 0; i < allBuildings.Length; i++)
        {
            if (allBuildings[i].GetComponent<BuildingMain>() != null)
            {
                string nameOfBuilding = allBuildings[i].GetComponent<BuildingMain>().buildingName;
                for(int p = 0; p < namesBuildings.Length; p++)
                {
                    if(nameOfBuilding == namesBuildings[p])
                    {
                        amountOfEachBuilding[p]++;
                    }
                }
            }
        }
    }

    public void WaitForInput(bool toChangeTo)
    {
        waitForInput = toChangeTo;
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
        stringToPush += GetFieldsToString() + "<DB>" + level + "<DB>" + money + "<DB>" + researchPoints + "<DB>" + DateTime.Now.ToString() + "<DB>" + GetQuestLines() + "<DB>" + exp + "<DB>" + nameTown + "<DB>" + GetHighscores() + "<DB>" + GameObject.Find("HUD").GetComponent<HUD>().notificationNumber + "<DB>" + GameObject.Find("SoundController").GetComponent<AudioSource>().mute.ToString() + "<DB>" + GameObject.Find("SFXController").GetComponent<AudioSource>().mute.ToString();
        saveLoad.writeStringToFile(stringToPush, "Documents");
    }
    string GetHighscores()
    {
        int[] allScores = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores;
        string scores = "";
        for(int i = 0; i < 3; i++)
        {
            scores += allScores[i].ToString();
            if(i != 2)
            {
                scores += "e";
            }
        }
        return scores;
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
        save = saveLoad.readStringFromFile("Documents");
        if(save == null)
        {
            GameObject.Find("Quests").GetComponent<Quests>().questLineProgress[0] = 1;
            GameObject.Find("Grid").GetComponent<Grid>().MakeGrid();
            if(!GameObject.Find("MiniGameController").GetComponent<MiniGameController>().fromClickToStart)
            {
                GoToScene("ClickToStart");
            }
        }
        else
        {
            PlaceBuildings();
            UpdateAmountOFBuildings();
            Destroy(GameObject.Find("Tutorial"));
        }
        GameObject.Find("Quests").GetComponent<Quests>().ResetQuests();
        if(GameObject.Find("MiniGameController").GetComponent<MiniGameController>().backFromMinigame)
        {
            GameObject.Find("MiniGameController").GetComponent<MiniGameController>().backFromMinigame = false;
            researchPoints += GameObject.Find("MiniGameController").GetComponent<MiniGameController>().currencyGotFromMinigame;
            int dialogToActivate = 12 + GameObject.Find("MiniGameController").GetComponent<MiniGameController>().difficultyMiniGame;
            GameObject.Find("Quests").GetComponent<Dialogs>().ActivateTalking(dialogToActivate);
        }
    }

    void PlaceBuildings()
    {
        GameObject[] allBuildingsToPlace = buildings.GetAllBuildings();
        GameObject.Find("Grid").GetComponent<Grid>().MakeGrid();
        string[] allInformation = Regex.Split(save, "<DB>");
        level = Convert.ToInt32(allInformation[1]);
        money = Convert.ToInt32(allInformation[2]);
        researchPoints = Convert.ToInt32(allInformation[3]);
        exp = Convert.ToInt32(allInformation[6]);
        nameTown = allInformation[7];

        string[] allScores = Regex.Split(allInformation[8], "e");
        for (int i = 0; i < 3; i++)
        {
            GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores[i] = Convert.ToInt32(allScores[i]);
        }
         
        GameObject.Find("HUD").GetComponent<HUD>().notificationNumber = Convert.ToInt32(allInformation[9]);
        GameObject.Find("HUD").GetComponent<HUD>().UpdateNotification(Convert.ToInt32(allInformation[9]));
        
        GameObject.Find("SoundController").GetComponent<AudioSource>().mute = Convert.ToBoolean(allInformation[10]);
        GameObject.Find("SFXController").GetComponent<AudioSource>().mute = Convert.ToBoolean(allInformation[11]);
        soundChecks[0].isOn = Convert.ToBoolean(allInformation[10]);
        soundChecks[1].isOn = Convert.ToBoolean(allInformation[11]);

        GameObject.Find("HUD").GetComponent<HUD>().SetName(nameTown);
        GameObject.Find("HUD").GetComponent<HUD>().ChangeBadge();

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
                            }
                        }

                        GameObject.Find("Grid").GetComponent<Grid>().grid[placeOfGridX, placeOfGridY].building = buildingToPlace.buildingName;
                        
                        ChangeColliders(true);
                        Vector2 size = buildingToPlace.size;
                        BuildingMain tempBuilding2 = (BuildingMain)Instantiate(buildingToPlace, GameObject.Find("Grid").GetComponent<Grid>().grid[placeOfGridX, placeOfGridY].transform.position, transform.rotation);
                        tempBuilding2.ID = GameObject.Find("Grid").GetComponent<Grid>().grid[placeOfGridX, placeOfGridY].ID;
                        tempBuilding2.transform.localScale = new Vector2(size.x * 0.98f, size.y * 0.95f);
                        //tempBuilding2.transform.localScale = new Vector2(3 * 0.98f, 3 * 0.95f);
                        if (tempBuilding2.GetComponent<BuildingMain>().buildingName != "TimeMachine")
                        {
                            tempBuilding2.taskDoing = Convert.ToInt32(informationOneBuilding[1]);
                            TimeSpan sec = DateTime.Now.Subtract(Convert.ToDateTime(allInformation[4]));
                            
                            if (Convert.ToBoolean(informationOneBuilding[2]))
                            {
                                tempBuilding2.busy = true;
                                //tempBuilding2.maxtimeForTask = Convert.ToInt32(informationOneBuilding[5]);
                            }
                            else
                            {
                                tempBuilding2.busy = false;
                            }
                            //tempBuilding2.maxtimeForTask = Convert.ToInt32(informationOneBuilding[2]);
                            tempBuilding2.level = Convert.ToInt32(informationOneBuilding[3]);
                            if (Convert.ToBoolean(informationOneBuilding[8]))
                            {
                                tempBuilding2.building = true;
                                //tempBuilding2.SetMaxTime();
                            }
                            else
                            {
                                tempBuilding2.building = false;
                            }
                            tempBuilding2.timer = Convert.ToDouble(informationOneBuilding[4]);
                            tempBuilding2.timer -= sec.TotalSeconds;
                            tempBuilding2.gridPosition.x = Convert.ToInt32(informationOneBuilding[6]);
                            tempBuilding2.gridPosition.y = Convert.ToInt32(informationOneBuilding[7]);
                        }
                        break;
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
            if (tempField.GetComponent<EmptyField>() != null)
            {
                tempField.GetComponent<EmptyField>().ChangeColor(Color.white);
                {
                    tempField.GetComponent<BoxCollider>().enabled = toChange;
                }
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
        //GameObject.Find("Quests").GetComponent<Quests>().enabled = toChange;
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
                        if (tempBuildings[k].GetComponent<BuildingMain>() != null)
                        {
                            if (tempBuildings[k].GetComponent<BuildingMain>().ID == grid[i, p].ID)
                            {
                                tempBuilding = tempBuildings[k].GetComponent<BuildingMain>();
                                break;
                            }
                        }
                    }
                }
                fields += grid[i, p].building + "<e>";
                if(tempBuilding != null)
                {
                    fields += tempBuilding.taskDoing + "<e>" + tempBuilding.busy + "<e>" + tempBuilding.level + "<e>" + tempBuilding.timer + "<e>" + tempBuilding.maxtimeForTask + "<e>" + tempBuilding.gridPosition.x + "<e>" + tempBuilding.gridPosition.y + "<e>" + tempBuilding.building;
                }
                fields += "<r>";
            }
        }
        return fields;
    }

    public void ResetSave()
    {
        string reset = GameObject.Find("InputField").GetComponent<InputField>().text;
        if (reset == "DELETE")
        {
            GameObject.Find("MiniGameController").GetComponent<MiniGameController>().fromClickToStart = false;
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("_Main");
        }
        if (reset == "TGM")
        {
            money += 1000;
            researchPoints += 1000;
        }
    }

    public void GoToScene(string scene)
    {
        if (scene == "Video")
        {
            Handheld.PlayFullScreenMovie("convert2.mp4");
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }

    public void ClickedLink(string link)
    {
        Application.OpenURL(link);
    }
}
