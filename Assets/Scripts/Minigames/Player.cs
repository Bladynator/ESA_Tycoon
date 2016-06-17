using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public int hp = 3;
    GameObject[] locations;
    int currentLocation = 2;
    bool waiting = false, waitingScore = false;
    public int endScore = 0;

    void Start()
    {
        locations = GameObject.Find("Controller").GetComponent<Controller>().spawns;
    }
    
    void Update()
    {
        transform.position = new Vector2(-6, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        /*
        transform.position = new Vector2(-8, locations[currentLocation].transform.position.y);
        if(!waitingScore)
        {
            StartCoroutine(WaitScore());
        }
        */
    }

    public void NewLocation(float value)
    {
        currentLocation = (int)value;
        StartCoroutine(Wait());
    }

    IEnumerator WaitScore()
    {
        waitingScore = true;
        yield return new WaitForSeconds(1);
        endScore++;
        waitingScore = false;
    }

    IEnumerator Wait()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;
    }
}
