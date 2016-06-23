using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{
    public int hp = 3;
    bool waitingScore = false;
    public int endScore = 0;
    Color oldColour;
    public Color redColour, redColourLast;
    public int diff;
    [SerializeField]
    AudioClip hit;

    void Start()
    {
        oldColour = GetComponent<SpriteRenderer>().color;
        GameObject.Find("Score").GetComponentInChildren<Text>().text = "Score: " + 0;
    }
    
    void Update()
    {
        transform.position = new Vector2(-6, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if(!waitingScore)
        {
            StartCoroutine(WaitScore());
        }
    }

    public void Hit()
    {
        GameObject.Find("SFXController").GetComponent<AudioSource>().PlayOneShot(hit);
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
                    endScore += 10;
                    hp = 1;
                    break;
                }
        }
        GameObject.Find("Score").GetComponentInChildren<Text>().text = "Score: " + endScore.ToString();
        waitingScore = false;
    }
}
