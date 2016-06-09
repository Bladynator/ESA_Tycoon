using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonToConnect : MonoBehaviour 
{
    public int number;
    public Controller3 controller;
    [SerializeField]
    GameObject liner;

    void Start()
    {
        GetComponentInChildren<Text>().text = number.ToString();
    }

    void OnMouseOver()
    {
        Clicked();
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
                controller.pressedButtonFirst = gameObject;
                controller.pressedButton = true;
                controller.numberToClick++;
                Instantiate(liner, transform.position, transform.rotation);
            }
        }
    }
}
