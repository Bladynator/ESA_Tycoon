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
    public int maxGridSize = 8, distance = 75;
    [SerializeField]
    Vector3 toTranslateAllFields = new Vector3(1,1,2);
    
    public void MakeGrid()
    {
        grid = new EmptyField[maxGridSize, maxGridSize];

        GameObject allFieldsTemp = Instantiate(allFields);
        
        for (int x = 0; x < maxGridSize; x++)
        {
            for (int y = 0; y < maxGridSize; y++)
            {
                Vector3 newPos = new Vector3((x - y) * 2.15f, (x + y) * 1.25f, 0);
                EmptyField tempField = (EmptyField)Instantiate(emptyField, newPos, emptyField.transform.rotation);
                tempField.transform.SetParent(allFieldsTemp.transform);
                tempField.ID = idToGive;
                tempField.buildMenu = GameObject.Find("BuilderMenu").GetComponent<BuilderMenu>();
                idToGive++;
                tempField.gridPosition = new Vector2(x, y);
                grid[x, y] = tempField;
            }
        }

        GameObject.Find("BuilderMenu").SetActive(false);
        allFieldsTemp.transform.position = new Vector3(-8.6f, 6.4f, 6.4f);
    }
}
