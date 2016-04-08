using UnityEngine;
using System.Collections;

public class ChangePositionBuilding : MonoBehaviour 
{
    BuildingPlacer buildingPlacer;
    [SerializeField]
    int newPosition;
    Vector2 newPostionOnGrid;

    void Start()
    {
        buildingPlacer = GetComponentInParent<BuildingPlacer>();
    }

    void OnMouseDown()
    {
        switch (newPosition)
        {
            case 1:
                {
                    newPostionOnGrid = new Vector2(buildingPlacer.activePlaceOnGrid.x += 1, buildingPlacer.activePlaceOnGrid.y);
                    break;
                }
            case 2:
                {
                    newPostionOnGrid = new Vector2(buildingPlacer.activePlaceOnGrid.x -= 1, buildingPlacer.activePlaceOnGrid.y);
                    break;
                }
            case 3:
                {
                    newPostionOnGrid = new Vector2(buildingPlacer.activePlaceOnGrid.x, buildingPlacer.activePlaceOnGrid.y += 1);
                    break;
                }
            case 4:
                {
                    newPostionOnGrid = new Vector2(buildingPlacer.activePlaceOnGrid.x, buildingPlacer.activePlaceOnGrid.y -= 1);
                    break;
                }
        }
        buildingPlacer.ChangePosition(newPostionOnGrid);
    }
}
