using UnityEngine;
using System.Collections;

public class Done : MonoBehaviour 
{
    BuildingPlacer buildingPlacer;
    int price;
    Account account;

    void Start()
    {
        buildingPlacer = GetComponentInParent<BuildingPlacer>();
        account = GameObject.Find("Account").GetComponent<Account>();
        price = buildingPlacer.buildingToPlace.GetComponent<BuildingMain>().price;
    }

    void OnMouseDown()
    {
        if (account.money >= price)
        {
            account.money -= price;
            buildingPlacer.Done();
        }
    }
}
