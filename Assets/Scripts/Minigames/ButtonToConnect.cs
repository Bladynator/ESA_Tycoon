using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonToConnect : MonoBehaviour 
{
    public int number;
    public Controller3 controller;
    
    Ray ray;
    RaycastHit hit;
    

    void Start()
    {
        GetComponentInChildren<Text>().text = (number + 1).ToString();
    }
    
    void OnMouseOver()
    {
        Clicked();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "End")
        {
            Clicked();
        }
    }
    
    public void Clicked()
    {
        if (number == controller.numberToClick)
        {
            if (controller.pressedButton)
            {
                controller.Calculate(gameObject);
            }
            else
            {
                controller.MakeRope();
            }
        }
    }

    
}
