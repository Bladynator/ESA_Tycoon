using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour 
{
	public void ToGame()
    {
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().fromClickToStart = true;

#if UNITY_ANDROID || UNITY_IOS
                Handheld.PlayFullScreenMovie("convert2.mp4");
#else

#endif
        SceneManager.LoadScene("_Main");
    }
}
