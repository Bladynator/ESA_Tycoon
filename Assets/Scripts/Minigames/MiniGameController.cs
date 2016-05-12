using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MiniGameController : MonoBehaviour 
{
    public int difficultyMiniGame = 0;
    public int score;
    public bool backFromMinigame = false;

	void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	public void ActivateMiniGame(string minigame, int difficulty)
    {
        difficultyMiniGame = difficulty;
        SceneManager.LoadScene(minigame);
        if(minigame == "_Main")
        {
            backFromMinigame = true;
        }
    }
}
