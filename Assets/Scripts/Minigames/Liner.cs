using UnityEngine;
using System.Collections;

public class Liner : MonoBehaviour 
{
    float timer = 10;
    bool done = false;
	
	void Start () 
	{
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
	}
	
	void Update () 
	{
        if (!done)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, 0f);
            transform.position = newPos;
            GetComponent<LineRenderer>().SetPosition(1, newPos);
        }
    }

    void FixedUpdate()
    {
        if (!done)
        {
            timer--;
            if (timer <= 0)
            {
                Instantiate(this.gameObject, transform.position, transform.rotation);
                done = true;
            }
        }
    }
}
