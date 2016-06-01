using UnityEngine;
using System.Collections;
using System;

public class Grid : MonoBehaviour
{
    public EmptyField[,] grid;
    int idToGive = 1;
    [SerializeField]
    EmptyField emptyField;
    [SerializeField]
    GameObject allFields;
    public int maxGridSize = 8;
    [SerializeField]
    BuildingButtons buildings;

    public void MakeGrid()
    {
        grid = new EmptyField[maxGridSize, maxGridSize];

        GameObject allFieldsTemp = Instantiate(allFields);
        
        for (int x = 0; x < maxGridSize; x++)
        {
            for (int y = 0; y < maxGridSize; y++)
            {
                Vector3 newPos = new Vector3((x - y) * 2.09f, (x + y) * 1.19f, 0);
                EmptyField tempField = (EmptyField)Instantiate(emptyField, newPos, emptyField.transform.rotation);
                tempField.transform.SetParent(allFieldsTemp.transform);
                tempField.ID = idToGive;
                tempField.buildMenu = buildings;
                idToGive++;
                tempField.gridPosition = new Vector2(x, y);
                grid[x, y] = tempField;
            }
        }
        
        allFieldsTemp.transform.position = new Vector3(-8.6f, 6.4f, 6.4f);
    }
}
