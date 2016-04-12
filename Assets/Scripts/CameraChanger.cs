using UnityEngine;
using System.Collections;

public class CameraChanger : MonoBehaviour 
{
    Camera camera;
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;

    void Start () 
	{
        camera = GetComponent<Camera>();
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hit_position = Input.mousePosition;
            camera_position = transform.position;

        }
        if (Input.GetMouseButton(0))
        {
            current_position = Input.mousePosition;
            LeftMouseDrag();
        }
    }

    void LeftMouseDrag()
    {
        current_position.z = hit_position.z = camera_position.y;
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);
        direction = direction * -1;
        Vector3 position = camera_position + direction;
        transform.position = position;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 40, 40), "+"))
        {
            camera.orthographicSize -= 2;
        }
        if (GUI.Button(new Rect(0, 40, 40, 40), "-"))
        {
            camera.orthographicSize += 2;
        }
    }
}
