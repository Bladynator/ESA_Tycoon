using UnityEngine;
using System.Collections;

public class Building1 : BuildingMain
{
    int[,] priceForUpgrading = new int[5, 3] // level, money, RP
        { { 1, 100, 0}, // level 1
        { 3 , 300 , 0}, // level 2
        { 5, 500, 10}, // level 3
        { 8, 1000, 50}, // level 4
        { 10 , 2000, 100 }}; // level 5

    public override void Start () 
	{
        base.Start();
        GiveInformation(priceForUpgrading);
    }

    public override void Update () 
	{
        base.Update();
	}
}
