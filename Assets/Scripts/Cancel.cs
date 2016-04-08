using UnityEngine;
using System.Collections;

public class Cancel : MonoBehaviour 
{
	
	void OnMouseDown()
    {
        Destroy(GetComponentInParent<GameObject>());
    }
}
