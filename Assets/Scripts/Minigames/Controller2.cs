using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller2 : MonoBehaviour
{
    public float[] resources = new float[3] { 100, 100, 100 };
    public float[,] reduceAmount = new float[4, 3]
        {   {0.06f,0.06f,0.06f},
            {0.08f,0.08f,0.08f},
            {0.10f,0.10f,0.10f},
            {0.15f,0.15f,0.15f} };
    public float[,] increaseAmount = new float[4,3]
        {   {0.08f,0.08f,0.08f},
            {0.16f,0.16f,0.16f},
            {0.20f,0.20f,0.20f},
            {0.25f,0.25f,0.25f} };
    [SerializeField]
    Image[] bars;
    public bool charging = false, overcharging = false;
    public int theOneToCharge = -1;

    bool waitingForTimer = false;
    int timer = 30;
    bool endLess = false, end = false;

    public int difficulty = 0, score = 0;

    [SerializeField]
    GameObject canvasEnd;
    GameObject tempCanvas;

    void Start()
    {
        EditTimer("Time: " + timer.ToString());
        difficulty = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().difficultyMiniGame;
    }

    void Update()
    {
        if (!end)
        {
            for (int i = 0; i < 3; i++)
            {
                bars[i].fillAmount = resources[i] / 100;
            }
            for (int i = 0; i < 3; i++)
            {
                if (resources[i] <= 0)
                {
                    End(Mathf.FloorToInt((resources[0] + resources[1] + resources[2]) / 2));
                }
            }
            if (!endLess)
            {
                if (!waitingForTimer)
                {
                    StartCoroutine(Timer());
                    waitingForTimer = true;
                }
                if (timer <= 0)
                {
                    End(Mathf.FloorToInt(resources[0] + resources[1] + resources[2]));
                }
            }
        }
    }

    void End(int scoreTotal)
    {
        float lastScore = scoreTotal;
        switch(difficulty)
        {
            case 0:
                {
                    lastScore /= 1.5f;
                    break;
                }
            case 1:
                {
                    lastScore /= 1f;
                    break;
                }
            case 2:
                {
                    lastScore /= 0.7f;
                    break;
                }
            case 3:
                {
                    lastScore /= 0.4f;
                    break;
                }
        }
        GameObject.Find("SFXController").GetComponent<AudioSource>().Stop();
        Destroy(GameObject.Find("SFXController"));
        end = true;
        score = Mathf.FloorToInt(lastScore);
        tempCanvas = Instantiate(canvasEnd);
        tempCanvas.GetComponentInChildren<Text>().text = "Score: " + score.ToString();
        tempCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedEnd(); });
    }

    void PressedEnd()
    {
        if (PlayerPrefs.GetInt("Minigame2") < score)
        {
            PlayerPrefs.SetInt("Minigame2", score);
        }
        PlayerPrefs.SetInt("RP", PlayerPrefs.GetInt("RP") + score);
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().ActivateMiniGame("_Main", difficulty);
    }

    void FixedUpdate()
    {
        for (int i = 0; i < 3; i++)
        {
            resources[i] -= reduceAmount[difficulty,i];
        }
        if(charging)
        {
            Charge(theOneToCharge);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        timer--;
        if(timer <= 0)
        {
            timer = 0;
        }
        EditTimer("Time: " + timer.ToString());
        waitingForTimer = false;
    }

    void EditTimer(string text)
    {

        GameObject.Find("Canvas").GetComponentInChildren<Text>().text = text;
    }

    public void Charge(int number)
    {
        int overcharge = 1;
        if(overcharging)
        {
            overcharge = 2;
        }
        resources[number] += (increaseAmount[difficulty, number] * overcharge);
    }
}
