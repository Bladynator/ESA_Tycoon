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
            GameObject.Find("MiniGameController").GetComponent<MiniGameController>().fromClickToStart = false;
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("_Main");
        }
        if (reset == "TGM")
        {
            GameObject.Find("Account").GetComponent<Account>().money += 1000;
            GameObject.Find("Account").GetComponent<Account>().researchPoints += 1000;
        }
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
}
