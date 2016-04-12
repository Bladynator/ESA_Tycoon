using UnityEngine;
using System.Collections;

public class Cancel : MonoBehaviour 
{
	void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("Builder").GetComponent<BuildingPlacer>().ChangeColliders(true);
        Destroy(GameObject.FindGameObjectWithTag("Builder"));
    }
}
