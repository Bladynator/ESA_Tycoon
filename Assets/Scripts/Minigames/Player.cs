using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public int hp = 3;
    GameObject[] locations;
    int currentLocation = 2;
    bool waiting = false, waitingScore = false;
    public int endScore = 0;
    Color oldColour;
    public Color redColour, redColourLast;
    public int diff;

    void Start()
    {
        locations = GameObject.Find("Controller").GetComponent<Controller>().spawns;
        oldColour = GetComponent<SpriteRenderer>().color;
    }
    
    void Update()
    {
        transform.position = new Vector2(-6, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        /*
        transform.position = new Vector2(-8, locations[currentLocation].transform.position.y);
        */
        if(!waitingScore)
        {
            StartCoroutine(WaitScore());
        }
    }

    public void Hit()
    {
        hp--;
        switch(hp)
        {
            case 2:
                {
                    GetComponent<SpriteRenderer>().color = redColour;
                    break;
                }
            case 1:
                {
                    GetComponent<SpriteRenderer>().color = redColourLast;
                    break;
                }
            case 0:
                {
                    GameObject.Find("Controller").GetComponent<Controller>().Ending();
                    break;
                }
        }
        
        StartCoroutine(WaitToTurnBack());
    }

    IEnumerator WaitToTurnBack()
    {
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().color = oldColour;
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
        switch(diff)
        {
            case 0:
                {
                    endScore += 3;
                    break;
                }
            case 1:
                {
                    endScore += 5;
                    break;
                }
            case 2:
                {
                    endScore += 7;
                    break;
                }
            case 3:
                {
                    endScore += 7;
                    hp = 1;
                    break;
                }
        }
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
