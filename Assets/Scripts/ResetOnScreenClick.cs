using UnityEngine;
using System.Collections;

public class ResetOnScreenClick : MonoBehaviour 
{
	void OnMouseDown()
    {
        Debug.Log("te");
        GameObject.Find("BuilderMenu").GetComponent<BuilderMenu>().Reset();
    }
}
