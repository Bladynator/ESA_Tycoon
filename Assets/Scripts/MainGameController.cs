using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainGameController : MonoBehaviour
{

	public static void ChangeColliders(bool toChange)
    {
        GameObject[] allFields = GameObject.FindGameObjectsWithTag("EmptyField");
        foreach (GameObject tempField in allFields)
        {
            if (tempField.GetComponent<EmptyField>() != null)
            {
                tempField.GetComponent<EmptyField>().ChangeColor(Color.white);
                {
                    tempField.GetComponent<BoxCollider>().enabled = toChange;
                }
            }
        }
        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject tempField in allBuildings)
        {
            if (tempField.GetComponent<CircleCollider2D>() != null)
            {
                tempField.GetComponent<CircleCollider2D>().enabled = toChange;
            }
        }
        if (toChange)
        {
            GameObject.Find("HUD").GetComponent<HUD>().EnableButton();
        }
    }

    public static void ResetSave()
    {
        string reset = GameObject.Find("InputField").GetComponent<InputField>().text;
        if (reset == "DELETE")
        {
            DeleteSave();
        }
        if (reset == "TGM")
        {
            DeleteSave();
        }
        if (reset == "TarunSucks")
        {
            GameObject.Find("Account").GetComponent<Account>().money += 5000;
            GameObject.Find("Account").GetComponent<Account>().researchPoints += 5000;
        }
    }

    public static void DeleteSave()
    {
        GameObject.Find("MiniGameController").GetComponent<MiniGameController>().fromClickToStart = false;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("_Main");
    }

    public static void CanMove(bool canmove)
    {
        GameObject.Find("Main Camera").GetComponent<CameraChanger>().canMove = canmove;
    }

    public static void GoToScene(string scene)
    {
        if (scene == "Video")
        {
#if UNITY_ANDROID || UNITY_IOS
                Handheld.PlayFullScreenMovie("convert2.mp4");
#else

#endif

        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }

    public static void Background()
    {
#if UNITY_ANDROID
        Application.runInBackground = true;
#else

#endif
    }
}
