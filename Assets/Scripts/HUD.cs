using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour 
{
    Account account;
    public Texture2D background, emptyBar, fullBar, taskDone;
    public BuilderMenu buildMenu;
    public bool buildClicked = false;

    void Start () 
	{
        account = GameObject.Find("Account").GetComponent<Account>();
	}

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, 0, 100, 25), "Level: " + account.level.ToString());
        GUI.Label(new Rect(Screen.width / 1.3f, 0, 100, 25), "Money: ");
        GUI.Label(new Rect(Screen.width / 1.1f, 0, 100, 25), account.money.ToString());
        GUI.Label(new Rect(Screen.width / 1.3f, 25, 150, 25), "ResearchPoints: ");
        GUI.Label(new Rect(Screen.width / 1.1f, 25, 100, 25), account.researchPoints.ToString());
        if (!buildClicked)
        {
            if (GUI.Button(new Rect(0, Screen.height - 50, 50, 50), "Build"))
            {
                GameObject obj = GameObject.Find("Main Camera").GetComponent<CameraChanger>().Field();
                if (obj != null)
                {
                    if (obj.GetComponent<EmptyField>() != null)
                    {
                        buildMenu.gameObject.SetActive(true);
                        buildMenu.fieldLocation = obj.GetComponent<EmptyField>().transform;
                        buildMenu.fieldID = obj.GetComponent<EmptyField>().ID;
                        buildMenu.fieldGridLocation = obj.GetComponent<EmptyField>().gridPosition;
                        buildClicked = true;
                    }
                }
            }
        }
    }
}
