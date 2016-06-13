using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonToConnect : MonoBehaviour 
{
    public int number;
    public Controller3 controller;
    [SerializeField]
    GameObject liner;
    public GameObject tempLiner;

    void Start()
    {
        GetComponentInChildren<Text>().text = number.ToString();
    }

    void OnMouseDown()
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
                if (tempLiner == null)
                {
                    tempLiner = (GameObject)Instantiate(liner, transform.position, transform.rotation);
                    //tempLiner.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //tempLiner.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                }
            }
        }
    }
}
