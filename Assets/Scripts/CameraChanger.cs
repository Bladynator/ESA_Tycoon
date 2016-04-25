using UnityEngine;
using System.Collections;

public class CameraChanger : MonoBehaviour 
{
    public Camera camera;
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;

    void Start () 
	{
        camera = GetComponent<Camera>();
	}

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Canvas") == null)
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
            transform.position = new Vector3(transform.position.x, transform.position.y, -26f);
        }
    }

    public GameObject Field()
    {
        Ray ray = new Ray(transform.position, transform.forward * 1000);
        RaycastHit hit = new RaycastHit();
        GameObject obj = null;
        
        if (Physics.Raycast (ray, out hit))
        {
            obj = hit.transform.gameObject;
            if (obj.GetComponent<EmptyField>() != null)
            {
                return obj;
            }
        }
        return null;
    }

    void LeftMouseDrag()
    {
        current_position.z = hit_position.z = camera_position.y;
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);
        direction = direction * -1;
        Vector3 position = camera_position + direction;
        transform.position = position;
        if (transform.position.x > 13)
        {
            transform.position = new Vector2(13, position.y);
        }
        if (transform.position.x < -32)
        {
            transform.position = new Vector2(-32, position.y);
        }
        if (transform.position.y > 33)
        {
            transform.position = new Vector2(position.x, 33);
        }
        if (transform.position.y < 10)
        {
            transform.position = new Vector2(position.x, 10);
        }
    }

    void OnGUI()
    {
        if (camera.orthographicSize > 4)
        {
            if (GUI.Button(new Rect(0, 0, 40, 40), "+"))
            {
                camera.orthographicSize -= 2;
            }
        }
        if (camera.orthographicSize < 19)
        {
            if (GUI.Button(new Rect(0, 40, 40, 40), "-"))
            {
                camera.orthographicSize += 2;
            }
        }
    }
}
