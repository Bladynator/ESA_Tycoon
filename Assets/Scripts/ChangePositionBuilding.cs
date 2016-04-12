﻿using UnityEngine;
using System.Collections;

public class ChangePositionBuilding : MonoBehaviour 
{
    BuildingPlacer buildingPlacer;
    [SerializeField]
    Vector2 newPostionOnGrid;
    int gridSize;

    void Start()
    {
        buildingPlacer = GetComponentInParent<BuildingPlacer>();
    }

    void OnMouseDown()
    {
        gridSize = GameObject.Find("Grid").GetComponent<Grid>().maxGridSize;
        if (buildingPlacer.activePlaceOnGrid.x + newPostionOnGrid.x >= 0 && buildingPlacer.activePlaceOnGrid.y + newPostionOnGrid.y >= 0 && buildingPlacer.activePlaceOnGrid.x + newPostionOnGrid.x <= gridSize - buildingPlacer.buildingToPlace.GetComponent<BuildingMain>().size.x - 1 && buildingPlacer.activePlaceOnGrid.y + newPostionOnGrid.y <= gridSize - buildingPlacer.buildingToPlace.GetComponent<BuildingMain>().size.y + 1)
        {
            newPostionOnGrid = new Vector2(buildingPlacer.activePlaceOnGrid.x += newPostionOnGrid.x, buildingPlacer.activePlaceOnGrid.y += newPostionOnGrid.y);
            buildingPlacer.ChangePosition(newPostionOnGrid);
        }
    }
}
