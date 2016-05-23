using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadgeSorter : MonoBehaviour 
{
    [SerializeField]
    GameObject badges;
    [SerializeField]
    Account account;
    Image[] allBadges;

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
            allBadges[i].gameObject.SetActive(true);
        }
        for (int i = account.level; i < 10; i++)
        {
            allBadges[i].gameObject.SetActive(false);
        }
    }
}
