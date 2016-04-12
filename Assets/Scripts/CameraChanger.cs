using UnityEngine;
using System.Collections;

public class CameraChanger : MonoBehaviour 
{
    Camera camera;
	
	void Start () 
	{
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	void OnGUI()
    {
        if(GUI.Button(new Rect(40,0,40,40), "UP"))
        {
            camera.transform.Translate(new Vector3(0, 6, 0));
        }
        if (GUI.Button(new Rect(0, 40, 40, 40), "Left"))
        {
            camera.transform.Translate(new Vector3(-6, 0, 0));
        }
        if (GUI.Button(new Rect(40, 40, 40, 40), "Down"))
        {
            camera.transform.Translate(new Vector3(0, -6, 0));
        }
        if (GUI.Button(new Rect(80, 40, 40, 40), "Right"))
        {
            camera.transform.Translate(new Vector3(6, 0, 0));
        }
        if (GUI.Button(new Rect(120, 0, 40, 40), "+"))
        {
            camera.orthographicSize -= 2;
        }
        if (GUI.Button(new Rect(120, 40, 40, 40), "-"))
        {
            camera.orthographicSize += 2;
        }
    }
}
