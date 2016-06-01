using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
    Account account;
    public BuildingButtons buildMenu;
    public Button buildButton;
    public Text money;
    public Text researchPoints;
    public Text level;
    public Text cityName;
    public Text exp;
    public Image expBar;
    public GameObject[] canvas;
    public GameObject[] buildingCanvas;
    public GameObject reset;

    void Start () 
	{
        account = GameObject.Find("Account").GetComponent<Account>();
        Input.simulateMouseWithTouches = true;
    }

    void Update()
    {
        money.text = account.money.ToString();
        researchPoints.text = account.researchPoints.ToString();
        level.text = "LVL " + account.level.ToString();
        exp.text = account.exp + "/" + account.expNeededForLevel[account.level] + " XP";
        float amount = (float)account.exp / (float)account.expNeededForLevel[account.level];
        expBar.fillAmount = amount;
    }

    public void SetName(string name)
    {
        cityName.text = name;
        canvas[2].SetActive(true);
        canvas[3].SetActive(true);
        account.nameTown = name;
    }
    
    public void EnableBuildMenu()
    {
        buildButton.interactable = false;
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject tempField in allBuildings)
        {
            if (tempField.GetComponent<CircleCollider2D>() != null)
            {
                tempField.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        GameObject obj = GameObject.Find("Main Camera").GetComponent<CameraChanger>().Field();
        if (obj == null)
        {
            EnableButton();
        }
    }

    public void CanvasEnabled(bool enabled)
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(enabled);
        }
    }
    
    public void EnableButton(bool enable = true)
    {
        if(!enable)
        {
            account.ChangeColliders(false);
        }
        buildButton.interactable = enable;
    }
}
