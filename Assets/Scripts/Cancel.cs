using UnityEngine;
using System.Collections;

public class Cancel : MonoBehaviour 
{
	public void OnMouseDown()
    {
        
        GameObject.FindGameObjectWithTag("Builder").GetComponent<BuildingPlacer>().Delete(false);
        GameObject.Find("Account").GetComponent<Account>().ChangeColliders(true);
        Destroy(GameObject.FindGameObjectWithTag("Builder"));
        if (GameObject.Find("Tutorial") != null)
        {
            if (!GameObject.Find("Tutorial").GetComponent<Tutorial>().tutorialDoing)
            {
                GameObject.Find("Account").GetComponent<Account>().PushSave();
                GameObject.Find("Account").GetComponent<Account>().autoSave = true;
            }
        }
        else
        {
            GameObject.Find("Account").GetComponent<Account>().PushSave();
            GameObject.Find("Account").GetComponent<Account>().autoSave = true;
        }
    }
}
