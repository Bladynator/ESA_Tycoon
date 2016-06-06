using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgeSorter : MonoBehaviour 
{
    [SerializeField]
    GameObject badges, infoscreen;
    [SerializeField]
    Account account;
    Image[] allBadges;
    [SerializeField]
    Text textToDisplay;
    string[] allText = new string[10]
        {"1","2","3","4","5","6","7","8","9","10" };

    void Start()
    {
        allBadges = badges.GetComponentsInChildren<Image>();
        Recount();
    }

    void Update()
    {
        if(account.justLeveld)
        {
            account.justLeveld = false;
            Recount();
        }
    }

    public void Recount()
    {
        for (int i = 0; i < account.level; i++)
        {
            string temp = allText[i];
            allBadges[i].gameObject.SetActive(true);
            allBadges[i].GetComponent<Button>().onClick.RemoveAllListeners();
            allBadges[i].GetComponent<Button>().onClick.AddListener(delegate { DisplayText(temp); });
        }
        for (int i = account.level; i < 10; i++)
        {
            allBadges[i].gameObject.SetActive(false);
        }
    }


    void DisplayText(string text)
    {
        infoscreen.SetActive(true);
        textToDisplay.text = text;
    }
}
