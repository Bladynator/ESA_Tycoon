using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToTycoon : MonoBehaviour 
{
    float level;
    [SerializeField]
    Image barToFill;

    public void Start()
    {
        level = GameObject.Find("MiniGameController").GetComponent<MiniGameController>().levelPlayer;
        barToFill.fillAmount = (float)(level / 10);
    }

	public void Back()
    {
        SceneManager.LoadScene("_Main");
    }
}
