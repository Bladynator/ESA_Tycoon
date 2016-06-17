using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour 
{
	public void ToGame()
    {
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().fromClickToStart = true;
        SceneManager.LoadScene("Video");
    }
}
