using UnityEngine;
using System.Collections;

public class Done : MonoBehaviour 
{
    BuildingPlacer buildingPlacer;
    int price;
    Account account;
    bool rePos;

    void Start()
    {
        buildingPlacer = GetComponentInParent<BuildingPlacer>();
        account = GameObject.Find("Account").GetComponent<Account>();
        price = buildingPlacer.buildingToPlace.GetComponent<BuildingMain>().price;
        rePos = buildingPlacer.rePos;
    }

    void OnMouseDown()
    {
        if (!rePos)
        {
            if (account.money >= price)
            {
                account.money -= price;
                buildingPlacer.Done();
            }
        }
        else
        {
            buildingPlacer.Done();
        }
    }
}
