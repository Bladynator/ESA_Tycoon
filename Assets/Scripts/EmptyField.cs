using UnityEngine;
using System.Collections;

public class EmptyField : MonoBehaviour
{
    public int ID;
    public Vector2 gridPosition;
    public BuildingButtons buildMenu;
    public bool placeAble = true;
    public string building = "EmptyField";
    
    public void ChangeColor(Color newColor)
    {
        GetComponent<SpriteRenderer>().color = newColor;
    }
}
