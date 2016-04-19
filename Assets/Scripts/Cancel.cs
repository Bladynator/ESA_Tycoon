using UnityEngine;
using System.Collections;

public class Cancel : MonoBehaviour 
{
	void OnMouseDown()
    {
        GameObject.Find("HUD").GetComponent<HUD>().enabled = true;
        GameObject.Find("Quests").GetComponent<Quests>().enabled = true;
        GameObject.FindGameObjectWithTag("Builder").GetComponent<BuildingPlacer>().ChangeColliders(true);
        Destroy(GameObject.FindGameObjectWithTag("Builder"));
    }
}
