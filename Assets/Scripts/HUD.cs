using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HUD : MonoBehaviour 
{
    Account account;
    public BuildingButtons buildMenu;
    public Toggle buildButton;
    public Text money;
    public Text researchPoints;
    public Text level;
    public Text cityName;
    public Text exp;
    public Image expBar;
    public GameObject[] canvas;
    public GameObject[] buildingCanvas;
    public GameObject reset;
    public GameObject particleUpgrade;
    public GameObject particleReward;
    public GameObject notification;
    public GameObject badges;
    public int notificationNumber = 2;
    public Sprite[] allBadgesImages;
    public Color disabledColour, disabledRed;
    public AudioClip upgradeSound, rewardSound, timelockSound;

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
        if (account.level != account.expNeededForLevel.Length)
        {
            exp.text = account.exp + "/" + account.expNeededForLevel[account.level] + " XP";
            float amount = (float)account.exp / (float)account.expNeededForLevel[account.level];
            expBar.fillAmount = amount;
        }
        else
        {
            exp.text = "Max Level";
        }
    }

    public void ChangeBadge()
    {
        GameObject.Find("LevelIcon").GetComponent<Image>().sprite = allBadgesImages[account.level - 1];
    }

    public void UpdateNotification(int newNumber = 90)
    {
        if (notificationNumber <= 0)
        {
            notification.SetActive(false);
        }
        else
        {
            notification.SetActive(true);
        }

        if (newNumber != 90)
        {
            notificationNumber = newNumber;
            notification.GetComponentInChildren<Text>().text = newNumber.ToString();
        }
        else
        {
            int old = Convert.ToInt32(notification.GetComponentInChildren<Text>().text);
            old--;
            notificationNumber = old;
            notification.GetComponentInChildren<Text>().text = old.ToString();
        }

        if (notificationNumber <= 0)
        {
            notification.SetActive(false);
        }
        else
        {
            notification.SetActive(true);
        }
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
            //account.ChangeColliders(false);
            MainGameController.ChangeColliders(false);
        }
        buildButton.interactable = enable;
    }
}
