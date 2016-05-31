using UnityEngine;
using System.Collections;

public class CameraChanger : MonoBehaviour 
{
    public Camera camera;
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;
    
    public float orthoZoomSpeed = 0.2f;        // The rate of change of the orthographic size in orthographic mode.
    
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

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            
            camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
            
            camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);

            if (camera.orthographicSize < 4)
            {
                camera.orthographicSize = 4;
            }
            if (camera.orthographicSize > 19)
            {
                camera.orthographicSize = 19;
            }
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
        if (transform.position.x < -12)
        {
            transform.position = new Vector2(-12, position.y);
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
}
