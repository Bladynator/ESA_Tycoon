using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Controller : MonoBehaviour 
{
    [SerializeField]
    GameObject negative;
    public GameObject[] spawns;
    bool waitingForSpawn = false;
    [SerializeField]
    float[] spawnSpeed;
    bool end = false;
    int endScore;
    [SerializeField]
    GameObject canvasEnd;
    GameObject tempCanvas;
    int difficulty;
    int lastLocation = 0;
    public float speedMultiplier = 1, currentTime = 0, currentTime2 = 0, startSpeed = 0;

    void Start()
    {
        difficulty = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().difficultyMiniGame;
        GameObject.Find("Player").GetComponent<Player>().diff = difficulty;
        startSpeed = spawnSpeed[difficulty];
    }
    
    void Update()
    {
        if (Time.time >= currentTime2 + startSpeed && !end)
        {
            currentTime2 = Time.time;
            for (int i = 0; i < 3; i++)
            {
                int randomHeight = 0;
                do
                {
                    randomHeight = Convert.ToInt32(Mathf.Floor(UnityEngine.Random.Range(0, spawns.Length)));
                }
                while (randomHeight == lastLocation);
                GameObject negativeTemp = (GameObject)Instantiate(negative, new Vector2(14, spawns[randomHeight].transform.position.y), this.transform.rotation);
                negativeTemp.GetComponent<Negative>().speedMultiplier = speedMultiplier;
                lastLocation = randomHeight;
            }
        }

        if (Time.time >= currentTime + 1f)
        {
            currentTime = Time.time;
            speedMultiplier += 0.02f;
            startSpeed -= 0.013f;
            if(startSpeed <= 0.2f)
            {
                startSpeed = 0.2f;
            }
        }
    }

    public void Ending()
    {
        end = true;
        GameObject[] all = GameObject.FindGameObjectsWithTag("Destroyer");
        foreach(GameObject temp in all)
        {
            Destroy(temp);
        }
        endScore = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().endScore;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        tempCanvas = Instantiate(canvasEnd);
        tempCanvas.GetComponentInChildren<Text>().text = "Score: " + endScore.ToString();
        tempCanvas.GetComponentInChildren<Button>().onClick.AddListener(delegate { PressedEnd(); });
    }
    
    void PressedEnd()
    {
        if(PlayerPrefs.GetInt("Minigame1") < endScore)
        {
            PlayerPrefs.SetInt("Minigame1", endScore);
        }
        PlayerPrefs.SetInt("RP", PlayerPrefs.GetInt("RP") + endScore);
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().ActivateMiniGame("_Main", difficulty);
    }
}
