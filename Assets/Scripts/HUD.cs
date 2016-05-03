using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
    Account account;
    public BuilderMenu buildMenu;
    public Button buildButton;
    public Text money;
    public Text researchPoints;
    public Text level;
    public Text cityName;
    public GameObject[] canvas;
    public GameObject[] buildingCanvas;

    void Start () 
	{
        account = GameObject.Find("Account").GetComponent<Account>();
        Input.simulateMouseWithTouches = true;
    }

    void Update()
    {
        money.text = account.money.ToString();
        researchPoints.text = account.researchPoints.ToString();
        level.text = account.level.ToString();
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
        buildButton.gameObject.SetActive(false);
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject tempField in allBuildings)
        {
            if (tempField.GetComponent<CircleCollider2D>() != null)
            {
                tempField.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        GameObject obj = GameObject.Find("Main Camera").GetComponent<CameraChanger>().Field();
        if (obj != null)
        {
            if (obj.GetComponent<EmptyField>() != null)
            {
                buildMenu.MakeButtons();
                buildMenu.fieldLocation = obj.GetComponent<EmptyField>().transform;
                buildMenu.fieldID = obj.GetComponent<EmptyField>().ID;
                buildMenu.fieldGridLocation = obj.GetComponent<EmptyField>().gridPosition;
            }
        }
        else
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
        buildButton.gameObject.SetActive(enable);
    }
}
