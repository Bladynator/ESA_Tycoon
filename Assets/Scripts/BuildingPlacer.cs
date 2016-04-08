using UnityEngine;
using System.Collections;

public class BuildingPlacer : MonoBehaviour 
{
    public GameObject buildingToPlace;
    public Vector2 activePlaceOnGrid;
    public BuildingPlacer builderPlacerTemp;
    public int fieldID;
    EmptyField[,] grid;
    bool placeAble = true;

    void Start()
    {
        ChangeColliders(false);
        grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
        for (int i = 0; i < buildingToPlace.GetComponent<BuildingMain>().size.x; i++)
        {
            for (int y = 0; y < buildingToPlace.GetComponent<BuildingMain>().size.y; y++)
            {
                Color newColor;
                if (grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].placeAble)
                {
                    newColor = Color.grey;
                }
                else
                {
                    newColor = Color.red;
                    placeAble = false;
                }
                grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].ChangeColor(newColor);

            }
        }
    }

    public void Done()
    {
        if (placeAble)
        {
            grid = GameObject.Find("Grid").GetComponent<Grid>().grid;
            for (int i = 0; i < buildingToPlace.GetComponent<BuildingMain>().size.x; i++)
            {
                for (int y = 0; y < buildingToPlace.GetComponent<BuildingMain>().size.y; y++)
                {
                    grid[(int)activePlaceOnGrid.x + y, (int)activePlaceOnGrid.y + i].placeAble = false;
                }
            }
        
            ChangeColliders(true);
            Vector2 size = buildingToPlace.GetComponent<BuildingMain>().size;
            Vector2 newPosition = transform.position;
            GameObject tempBuilding = (GameObject)Instantiate(buildingToPlace, newPosition, transform.rotation);

            tempBuilding.transform.localScale = size;
            Destroy(gameObject);
        }
    }
    
    void ChangeColliders(bool toChange)
    {
        GameObject[] allFields = GameObject.FindGameObjectsWithTag("EmptyField");
        foreach (GameObject tempField in allFields)
        {
            tempField.GetComponent<EmptyField>().Reset();
            if (tempField.GetComponent<EmptyField>().ID != fieldID)
            {
                tempField.GetComponent<BoxCollider2D>().enabled = toChange;
            }
        }
    }

    public void ChangePosition(Vector2 newPostion)
    {
        GameObject[] allFields = GameObject.FindGameObjectsWithTag("EmptyField");
        foreach (GameObject tempField in allFields)
        {
            if (tempField.GetComponent<EmptyField>().gridPosition == newPostion)
            {
                Vector3 positionOfNewBuilding = new Vector3(tempField.transform.position.x, tempField.transform.position.y + 0.3f, tempField.transform.position.z);
                BuildingPlacer tempBuilding = (BuildingPlacer)Instantiate(builderPlacerTemp, positionOfNewBuilding, transform.rotation);
                tempBuilding.buildingToPlace = buildingToPlace;
                tempBuilding.activePlaceOnGrid = activePlaceOnGrid;
                tempBuilding.builderPlacerTemp = builderPlacerTemp;
                tempBuilding.fieldID = tempField.GetComponent<EmptyField>().ID;
                
                Destroy(gameObject);
            }
        }
        
    }
}
