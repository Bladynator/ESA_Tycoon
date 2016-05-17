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

	void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	public void ActivateMiniGame(string minigame, int difficulty, int currencyGot = 0)
    {
        difficultyMiniGame = difficulty;
        SceneManager.LoadScene(minigame);
        if(minigame == "_Main")
        {
            backFromMinigame = true;
            currencyGotFromMinigame = currencyGot;
        }
    }
}
