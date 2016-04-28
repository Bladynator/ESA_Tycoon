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
        canvas[3].SetActive(true);
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
        buildButton.gameObject.SetActive(enable);
    }
}
