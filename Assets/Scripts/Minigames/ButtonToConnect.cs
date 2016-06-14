using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonToConnect : MonoBehaviour 
{
    public int number;
    public Controller3 controller;
    [SerializeField]
    GameObject liner, chainObject;
    [SerializeField]
    Material tempMat;
    GameObject tempLiner;
    Breakable[] temp2;
    Vector2 positionOld;
    Ray ray;
    RaycastHit hit;
    public Rope2D newRope = new Rope2D();

    void Start()
    {
        GetComponentInChildren<Text>().text = number.ToString();
    }

    void OnMouseOver()
    {
        Debug.Log("f");
        Clicked();
        
    }

    void Update()
    {
        if (temp2 != null)
        {
            temp2[temp2.Length - 1].transform.position = positionOld;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "End")
        {
            Clicked();
        }
    }

    public void Clicked()
    {
        Debug.Log("k");
        if (number == controller.numberToClick)
        {
            Debug.Log("j");
            if (controller.pressedButton)
            {
                Debug.Log("h");
                controller.Calculate(gameObject);
            }
            else
            {
                controller.pressedButtonFirst = gameObject;
                controller.pressedButton = true;
                controller.numberToClick++;
                tempLiner = Instantiate(liner);
                
                Transform[] childerenFromRope = tempLiner.GetComponentsInChildren<Transform>();
                tempLiner.transform.position = transform.position;
                childerenFromRope[0].transform.position = transform.position;
                childerenFromRope[1].transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newRope.CreateRope(tempLiner, chainObject, childerenFromRope[0], childerenFromRope[1], true, false, true, true, true, true, tempMat, 0.3f);
                temp2 = GameObject.Find("RopeNew(Clone)").GetComponentsInChildren<Breakable>();
                temp2[0].gameObject.AddComponent<ToMouse>();
                foreach (Breakable temp3 in temp2)
                {
                    temp3.enabled = false;
                    if(temp3.GetComponent<HingeJoint2D>() != null)
                    {
                        temp3.GetComponent<HingeJoint2D>().useLimits = false;
                    }
                    temp3.GetComponent<Rigidbody2D>().mass = 0;
                    temp3.GetComponent<Rigidbody2D>().gravityScale = 0;
                    temp3.gameObject.layer = 2;
                }
                positionOld = temp2[temp2.Length - 1].transform.position;

                temp2[temp2.Length - 1].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                temp2[temp2.Length - 1].GetComponent<Rigidbody2D>().mass = 90000;
                //Destroy(temp2[temp2.Length - 1].GetComponent<Rigidbody2D>());
                /*
                if (tempLiner == null)
                {
                    tempLiner = (GameObject)Instantiate(liner, transform.position, transform.rotation);
                    //tempLiner.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //tempLiner.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                }
                */
            }
        }
    }
}
