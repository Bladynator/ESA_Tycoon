using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour 
{
    public string minigameToLoad = "";
    [SerializeField]
    GameObject[] canvas;

	void Start () 
	{
        minigameToLoad = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().minigameToLoad;
        switch(minigameToLoad)
        {
            case "Minigame1":
                {
                    canvas[1].SetActive(true);
                    break;
                }
            case "Minigame2":
                {
                    canvas[2].SetActive(true);
                    break;
                }
            case "Minigame3":
                {
                    canvas[3].SetActive(true);
                    break;
                }
        }
	}

    public void PressedPlay()
    {
        SceneManager.LoadScene(minigameToLoad);
    }

    public void PressedBack()
    {
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().pressedBack = true;
        SceneManager.LoadScene("_Main");
    }
}
