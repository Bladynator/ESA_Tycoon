using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour 
{
    Account account;
    public Texture2D background, emptyBar, fullBar;
	
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
    }
}
