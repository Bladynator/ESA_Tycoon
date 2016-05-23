using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackToTycoon : MonoBehaviour 
{
	public void Back()
    {
        SceneManager.LoadScene("_Main");
    }
}
