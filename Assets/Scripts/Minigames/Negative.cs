using UnityEngine;
using System.Collections;

public class Negative : MonoBehaviour 
{
	
	
	void Update () 
	{
        transform.position -= new Vector3(0.1f, 0, 0);
        if(transform.position.x < - 10)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().hp--;
            if(other.GetComponent<Player>().hp <= 0)
            {
                GameObject.Find("Controller").GetComponent<Controller>().Ending();
            }
            Destroy(gameObject);
        }
    }
}
