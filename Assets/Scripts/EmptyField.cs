using UnityEngine;
using System.Collections;

public class EmptyField : MonoBehaviour
{
    bool isClicked = false;
    public int ID;
    public Vector2 gridPosition;
    public BuilderMenu buildMenu;
    public bool placeAble = true;
    public string building = "EmptyField";
    
    void OnMouseDown()
    {
        if (placeAble)
        {
            GameObject[] allFields = GameObject.FindGameObjectsWithTag("EmptyField");
            foreach (GameObject tempField in allFields)
            {
                if (tempField != this.gameObject)
                {
                    tempField.GetComponent<EmptyField>().Reset();
                }
            }
            if (!isClicked)
            {
                ChangeColor(Color.grey);
                isClicked = true;
                buildMenu.gameObject.SetActive(true);
                buildMenu.fieldLocation = transform;
                buildMenu.fieldID = ID;
                buildMenu.fieldGridLocation = gridPosition;
            }
            else
            {
                Reset();
            }
        }
    }

    public void ChangeColor(Color newColor)
    {
        GetComponent<SpriteRenderer>().color = newColor;
    }

    public void Reset()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        isClicked = false;
        buildMenu.gameObject.SetActive(false);
    }
}
