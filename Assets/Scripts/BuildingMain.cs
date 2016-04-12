using UnityEngine;
using System.Collections;

public class BuildingMain : MonoBehaviour 
{
    public string buildingName;
    public Vector2 size;
    public int price, levelNeeded, timeToBuild;
    public bool clicked, busy;
    Texture2D background, emptyBar, fullBar;
    public int level, ID, timeLeftToFinishBuild, taskDoing;
    [SerializeField]
    int[] timesForTasks;
    public float timeToFinishTask, timeToFinishTaskTotal;
    bool waitOneSec = false;

    public virtual void Start () 
	{
        background = GameObject.Find("HUD").GetComponent<HUD>().background;
        emptyBar = GameObject.Find("HUD").GetComponent<HUD>().emptyBar;
        fullBar = GameObject.Find("HUD").GetComponent<HUD>().fullBar;
    }

    public virtual void Update()
    {
        if(timeToFinishTask > 0)
        {
            busy = true;
            if(!waitOneSec)
            {
                StartCoroutine(WaitForTask());
            }
        }
        else
        {
            busy = false;
        }
    }

    IEnumerator WaitForTask()
    {
        waitOneSec = true;
        timeToFinishTask--;
        yield return new WaitForSeconds(1);
        waitOneSec = false;
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
    }

    void OnGUI()
    {
        if (clicked)
        {
            GUI.DrawTexture(new Rect(Screen.width / 3, Screen.height / 4, Screen.width / 3, Screen.height / 2), background);
            GUI.Label(new Rect(Screen.width / 3 + 60, Screen.height / 4, Screen.width / 3, Screen.height / 2), buildingName);
            GUI.Label(new Rect(Screen.width / 3 + 60, Screen.height / 2, Screen.width / 3, Screen.height / 2), level.ToString());
            if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 4, 50, 50), "Back"))
            {
                clicked = false;
            }
            for(int i = 0; i < timesForTasks.Length; i++)
            {
                if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 3 + (i * 50), 150, 50), (i + 1) + " Task " + timesForTasks[i] + " Sec"))
                {
                    timeToFinishTaskTotal = timesForTasks[i];
                    timeToFinishTask = timeToFinishTaskTotal;
                    GameObject.Find("Account").GetComponent<Account>().PushSave();
                    clicked = false;
                }
            }
        }
        if (busy)
        {
            Vector3 tempPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, -transform.position.y));
            GUI.DrawTexture(new Rect(tempPos.x - 25, tempPos.y - 80, 50, 15), emptyBar);
            float perc = timeToFinishTaskTotal / timeToFinishTask;
            GUI.DrawTexture(new Rect(tempPos.x - 23, tempPos.y - 78, 46 / perc, 11), fullBar);
        }
    }
}
