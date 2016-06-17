using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Controller3 : MonoBehaviour 
{
    [SerializeField]
    ButtonToConnect button;
    [SerializeField]
    Transform[] positionsOfButtons = new Transform[12];
    public GameObject pressedButtonFirst;
    public bool pressedButton = false;
    int totalConnected = 0;
    List<int> excludedNumbers = new List<int>();
    Quaternion rotation;
    public int amountToShow = 2, numberToClick = 0, difficulty = 0, score = 0;
    [SerializeField]
    int[] amountsToShow;
    bool endLess = false, end = false;
    int turn = 0;

    bool waitingForTimer = false;
    int timer = 30;
    bool holdingDown = true;

    [SerializeField]
    Sprite[] icons;

    [SerializeField]
    GameObject canvasEnd, CanvasTimer;
    GameObject tempCanvas;
    ButtonToConnect[] temp;

    public Rope2D newRope;
    GameObject tempLiner;
    Breakable[] temp2;
    Vector2 positionOld;
    [SerializeField]
    GameObject liner, chainObject;
    [SerializeField]
    Material tempMat;

    void Start()
    {
        difficulty = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().difficultyMiniGame;
        amountToShow = amountsToShow[difficulty];
        if(difficulty == 3)
        {
            endLess = true;
        }
        temp = new ButtonToConnect[amountToShow + 1];
        Place();
        EditTimer("Time: " + timer.ToString());
    }

    void Update()
    {
        if (temp2 != null)
        {
            temp2[temp2.Length - 1].transform.position = positionOld;
        }
        if (Input.touchCount == 0 && pressedButtonFirst != null)
        {
            //Calculate(pressedButtonFirst);
        }
        if(!endLess && !end)
        {
            if(!waitingForTimer)
            {
                StartCoroutine(Timer());
                waitingForTimer = true;
            }
            if(timer <= 0)
            {
                End();
            }
        }
    }

    void End()
    {
        end = true;
        ButtonToConnect[] all = FindObjectsOfType<ButtonToConnect>();
        foreach (ButtonToConnect temp in all)
        {
            Destroy(temp);
        }
        Liner[] all2 = FindObjectsOfType<Liner>();
        foreach (Liner temp in all2)
        {
            Destroy(temp);
        }
        tempCanvas = Instantiate(canvasEnd);
        tempCanvas.GetComponentInChildren<Text>().text = "Score: " + score.ToString();
        tempCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedEnd(); });
    }

    void PressedEnd()
    {
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().score = score;
        if (GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores[0] < score)
        {
            GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores[0] = score;
        }
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().ActivateMiniGame("_Main", difficulty, score);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        timer--;
        EditTimer("Time: " + timer.ToString());
        waitingForTimer = false;
    }

    void EditTimer(string text)
    {
        CanvasTimer.GetComponent<Text>().text = text;
    }

    void Place()
    {
        if(endLess)
        {
            turn++;
            if(turn >= 5)
            {
                amountToShow++;
            }
        }
        int number = 0;
        temp = new ButtonToConnect[amountToShow + 1];
        excludedNumbers = new List<int>();
        
        DeleteRope();
        
        for (int i = 0; i < amountToShow; i++)
        {
            do
            {
                number = Random.Range(0, positionsOfButtons.Length);
            }
            while (excludedNumbers.Contains(number));
            excludedNumbers.Add(number);

            temp[i] = (ButtonToConnect)Instantiate(button, positionsOfButtons[number].position, rotation);
            
            temp[i].tag = "Connect";
            temp[i].controller = GetComponent<Controller3>();
            temp[i].number = i;
        }
        bool g = true;
        foreach (ButtonToConnect temp2 in temp)
        {
            if (temp2 != null)
            {
                Image[] allsprites = temp2.GetComponentsInChildren<Image>();
                if (g)
                {
                    do
                    {
                        number = Random.Range(0, 20);
                    }
                    while (number % 2 != 0);
                    g = false;

                    allsprites[1].sprite = icons[number];
                }
                else
                {
                    g = true;
                    allsprites[1].sprite = icons[number + 1];
                }
            }
        }
    }

    public void MakeRope()
    {
        pressedButtonFirst = gameObject;
        pressedButton = true;
        numberToClick++;
        tempLiner = Instantiate(liner);

        Transform[] childerenFromRope = tempLiner.GetComponentsInChildren<Transform>();
        tempLiner.transform.position = transform.position;
        childerenFromRope[0].transform.position = transform.position;
        childerenFromRope[1].transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newRope = new Rope2D();
        newRope.CreateRope(tempLiner, chainObject, childerenFromRope[0], childerenFromRope[1], true, false, true, true, true, true, tempMat, 0.3f);
        temp2 = GameObject.Find("RopeNew(Clone)").GetComponentsInChildren<Breakable>();
        temp2[0].gameObject.AddComponent<ToMouse>();
        foreach (Breakable temp3 in temp2)
        {
            temp3.enabled = false;
            if (temp3.GetComponent<HingeJoint2D>() != null)
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
    }

    public void Calculate(GameObject button)
    {
        pressedButton = false;
        Vector3[] positions = new Vector3[2];
        positions[0] = pressedButtonFirst.transform.position;
        positions[1] = button.transform.position;
        pressedButtonFirst = null;
        if (numberToClick != amountToShow)
        {
            excludedNumbers.Clear();
            totalConnected = 0;
            GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("Connect");
            for (int i = 0; i < toDestroy.Length; i++)
            {
                Destroy(toDestroy[i]);
            }
            numberToClick = 0;
            Place();
            score += amountToShow;
            pressedButtonFirst = null;
            pressedButton = false;
        }
    }

    public void DeleteRope()
    {

        if (newRope != null)
        {
            Destroy(tempLiner);
            temp2 = null;
            newRope = null;
        }

    }
}
