using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
    Account account;
    public Texture2D background, emptyBar, fullBar, taskDone;
    public BuilderMenu buildMenu;
    public Button buildButton;
    public Text money;
    public Text researchPoints;
    public Text level;

    void Start () 
	{
        account = GameObject.Find("Account").GetComponent<Account>();
        Input.simulateMouseWithTouches = true;
    }

    void Update()
    {
        money.text = "Money: " + account.money;
        researchPoints.text = "ResearchPoints: " + account.researchPoints;
        level.text = "Level: " + account.level;
    }

    public void EnableBuildMenu()
    {
        buildButton.gameObject.SetActive(false);
        GameObject obj = GameObject.Find("Main Camera").GetComponent<CameraChanger>().Field();
        if (obj != null)
        {
            if (obj.GetComponent<EmptyField>() != null)
            {
                buildMenu.gameObject.SetActive(true);
                buildMenu.fieldLocation = obj.GetComponent<EmptyField>().transform;
                buildMenu.fieldID = obj.GetComponent<EmptyField>().ID;
                buildMenu.fieldGridLocation = obj.GetComponent<EmptyField>().gridPosition;
            }
        }
    }

    public void EnableButton()
    {
        buildButton.gameObject.SetActive(true);
    }
}
