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
    List<int> excludedNumbers = new List<int>();
    public int amountToShow = 2, numberToClick = 0, difficulty = 0, score = 0;
    bool endLess = false, end = false;
    int turn = 0;

    bool waitingForTimer = false;
    int timer = 30;

    [SerializeField]
    Sprite[] icons;

    [SerializeField]
    GameObject canvasEnd, CanvasTimer;
    GameObject tempCanvas;
    ButtonToConnect[] temp;

    public Rope2D newRope;
    [SerializeField]
    GameObject chainObject;
    bool touching = true;
    [SerializeField]
    AudioClip hit, done;

    void Start()
    {
        difficulty = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().difficultyMiniGame;
        amountToShow = 2;
        /*
        if(difficulty == 3)
        {
            endLess = true;
        }
        */
        temp = new ButtonToConnect[amountToShow + 1];
        Place();
        EditTimer("Time: " + timer.ToString());
    }

    void Update()
    {
        if (!endLess && !end)
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
        if(pressedButton)
        {
            DeleteRope();
            MakeRopeFinal();
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
        switch(difficulty)
        {
            case 0:
                {
                    score = score + 0;
                    break;
                }
            case 1:
                {
                    score = score + 20;
                    break;
                }
            case 2:
                {
                    score = score + 40;
                    break;
                }
            case 3:
                {
                    score = score + 60;
                    break;
                }
        }
        tempCanvas.GetComponentInChildren<Text>().text = "Score: " + score.ToString();
        tempCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedEnd(); });
    }

    void PressedEnd()
    {
        if (PlayerPrefs.GetInt("Minigame3") < score)
        {
            PlayerPrefs.SetInt("Minigame3", score);
        }
        PlayerPrefs.SetInt("RP", PlayerPrefs.GetInt("RP") + score);
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().ActivateMiniGame("_Main", difficulty);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        timer--;
        if (timer <= 0)
        {
            timer = 0;
        }
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

            temp[i] = (ButtonToConnect)Instantiate(button, positionsOfButtons[number].position, positionsOfButtons[number].rotation);
            
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
        GameObject.Find("SFXController").GetComponent<AudioSource>().PlayOneShot(hit);
        pressedButtonFirst = gameObject;
        pressedButton = true;
        numberToClick++;
        MakeRopeFinal();
    }

    void MakeRopeFinal()
    {
        Instantiate(chainObject, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 30), transform.rotation);
    }

    public void Calculate(GameObject button)
    {
        GameObject.Find("SFXController").GetComponent<AudioSource>().PlayOneShot(done);
        pressedButton = false;
        Vector3[] positions = new Vector3[2];
        positions[0] = pressedButtonFirst.transform.position;
        positions[1] = button.transform.position;
        pressedButtonFirst = null;
        GameObject[] allRope = GameObject.FindGameObjectsWithTag("rope2D");
        for (int i = 0; i < allRope.Length; i++)
        {
            Destroy(allRope[i]);
        }
        
        if (touching)
        {
            if (numberToClick  != amountToShow)
            {
                excludedNumbers.Clear();
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
    }

    public void DeleteRope()
    {
        if (newRope != null)
        {
            newRope = null;
        }
    }
}
