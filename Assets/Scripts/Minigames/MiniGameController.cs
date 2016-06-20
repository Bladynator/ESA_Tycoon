using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MiniGameController : MonoBehaviour 
{
    public int difficultyMiniGame = 0;
    public int score;
    public int currencyGotFromMinigame = 0;
    public bool backFromMinigame = false;
    public int[] highscores = new int[3] {0,0,0 };
    public string minigameToLoad = "";

    public bool fromClickToStart = false;
    public int levelPlayer = 1;
    public int buildingID;
    public bool pressedBack = false;

	void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
	
	public void ActivateMiniGame(string minigame, int difficulty, int currencyGot = 0)
    {
        difficultyMiniGame = difficulty;
        minigameToLoad = minigame;
        if(minigame == "_Main")
        {
            SceneManager.LoadScene("_Main");
            backFromMinigame = true;
            currencyGotFromMinigame = currencyGot;
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}
