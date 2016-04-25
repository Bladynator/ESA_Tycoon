using UnityEngine;
using System.Collections;

public class Cancel : MonoBehaviour 
{
	void OnMouseDown()
    {
        GameObject.Find("Account").GetComponent<Account>().ChangeColliders(true);
        Destroy(GameObject.FindGameObjectWithTag("Builder"));
    }
}
