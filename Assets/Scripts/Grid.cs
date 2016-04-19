using UnityEngine;
using System.Collections;

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
                Vector2 location = new Vector2(x * distance, y * distance);
                EmptyField tempField = (EmptyField)Instantiate(emptyField, Camera.main.ScreenToWorldPoint(location), emptyField.transform.rotation);
                tempField.transform.Translate(toTranslateAllFields);
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
        allFieldsTemp.transform.rotation = transform.rotation;
    }
}
