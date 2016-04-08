using UnityEngine;
using System.Collections;

public class Done : MonoBehaviour 
{
    BuildingPlacer buildingPlacer;

    void Start()
    {
        buildingPlacer = GetComponentInParent<BuildingPlacer>();
    }

    void OnMouseDown()
    {
        buildingPlacer.Done();
    }
}
