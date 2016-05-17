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
    float[] spawnSpeed, reduction, endspeed;
    bool end = false;
    int endScore;
    [SerializeField]
    GameObject canvasEnd;
    GameObject tempCanvas;
    int difficulty;
    int lastLocation = 0;

    void Start()
    {
        difficulty = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().difficultyMiniGame;
    }
    
    void Update()
    {
        if (!waitingForSpawn && !end)
        {
            StartCoroutine(waitForSec(spawnSpeed[difficulty]));
        }
    }

    IEnumerator waitForSec(float sec)
    {
        waitingForSpawn = true;
        for (int i = 0; i < 2; i++)
        {
            int randomHeight = 0;
            do
            {
                randomHeight = Convert.ToInt32(Mathf.Floor(UnityEngine.Random.Range(0, spawns.Length)));
            }
            while (randomHeight == lastLocation);
            GameObject negativeTemp = (GameObject)Instantiate(negative, new Vector2(8, spawns[randomHeight].transform.position.y), this.transform.rotation);
            spawnSpeed[difficulty] -= reduction[difficulty];
            if (spawnSpeed[difficulty] <= endspeed[difficulty])
            {
                spawnSpeed[difficulty] = endspeed[difficulty];
            }
            lastLocation = randomHeight;
        }
        yield return new WaitForSeconds(sec);
        waitingForSpawn = false;
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
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().score = endScore;
        if(GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores[1] < endScore)
        {
            GameObject.Find("MiniGameController").GetComponent<MiniGameController>().highscores[1] = endScore;
        }
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().ActivateMiniGame("_Main", difficulty); // back to tycoon
    }
}
