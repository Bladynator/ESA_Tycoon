using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgeSorter : MonoBehaviour 
{
    [SerializeField]
    GameObject badges, infoscreen, link;
    [SerializeField]
    Account account;
    Image[] allBadges;
    [SerializeField]
    Sprite[] tempFull;
    [SerializeField]
    Sprite emptyBadge;
    [SerializeField]
    Text textToDisplay, textInformation;
    [SerializeField]
    Image tempBadge;
    [SerializeField]
    Sprite[] links;
    [SerializeField]
    string[] badgeName, badgeInfo, urls;

    void OnEnable()
    {
        allBadges = badges.GetComponentsInChildren<Image>();
        Recount();
    }

    public void Recount()
    {
        for (int i = 0; i < account.level; i++)
        {
            string temp = badgeName[i];
            string temp2 = badgeInfo[i];
            Sprite temp3 = tempFull[i];
            Sprite temp4 = links[i];
            string url = urls[i];
            allBadges[i].sprite = tempFull[i];
            allBadges[i].GetComponent<Button>().onClick.RemoveAllListeners();
            allBadges[i].GetComponent<Button>().onClick.AddListener(delegate { DisplayText(temp, temp2, temp3, temp4, url); });
        }
        for (int i = account.level; i < 10; i++)
        {
            allBadges[i].sprite = emptyBadge;
        }
    }


    void DisplayText(string text, string infoText, Sprite image, Sprite linkst, string url)
    {
        infoscreen.SetActive(true);
        textToDisplay.text = text;
        textInformation.text = infoText;
        tempBadge.sprite = image;
        link.GetComponent<Image>().sprite = linkst;
        string tempUrl = url;
        link.GetComponent<Button>().onClick.AddListener(delegate { ClickedLink(tempUrl); });
    }

    void ClickedLink(string link)
    {
        Application.OpenURL(link);
    }
}
