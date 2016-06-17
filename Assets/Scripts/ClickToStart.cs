using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour 
{
	public void ToGame()
    {
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().fromClickToStart = true;
        Handheld.PlayFullScreenMovie("convert2.mp4");
        SceneManager.LoadScene("_Main");
    }
}
