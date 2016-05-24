using UnityEngine;
using System.Collections;

public class Done : MonoBehaviour 
{
    BuildingPlacer buildingPlacer;
    int price, rpPrice;
    Account account;
    bool rePos;

    void Start()
    {
        buildingPlacer = GetComponentInParent<BuildingPlacer>();
        account = GameObject.Find("Account").GetComponent<Account>();
        price = buildingPlacer.buildingToPlace.GetComponent<BuildingMain>().moneyNeededUpgrade[0];
        rpPrice = buildingPlacer.buildingToPlace.GetComponent<BuildingMain>().rpNeededUpgrade[0];
        rePos = buildingPlacer.rePos;
    }

    void OnMouseDown()
    {
        if (buildingPlacer.placeAble)
        {
            if (!rePos)
            {
                if (account.money >= price && account.researchPoints >= rpPrice)
                {
                    account.researchPoints -= rpPrice;
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
}
