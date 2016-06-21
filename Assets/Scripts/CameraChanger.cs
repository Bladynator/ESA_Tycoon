using UnityEngine;
using System.Collections;

public class CameraChanger : MonoBehaviour 
{
    public Camera camera;
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;
    public bool activeDialog = false;
    
    public float orthoZoomSpeed = 0.1f;

    float touchDuration;
    Touch touch;

    public bool canMove = true;

    public bool builderOn = false;

    void Start () 
	{
        camera = GetComponent<Camera>();
	}

    void Update()
    {
        if (canMove)
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
                if (Input.GetMouseButtonUp(0) && !builderOn)
                {
                    StartCoroutine(WaitForSmallTime());
                }

                transform.position = new Vector3(transform.position.x, transform.position.y, -26f);
                if (transform.position.x > 33)
                {
                    transform.position = new Vector2(33, transform.position.y);
                }
                if (transform.position.x < -32)
                {
                    transform.position = new Vector2(-32, transform.position.y);
                }
                if (transform.position.y > 63)
                {
                    transform.position = new Vector2(transform.position.x, 63);
                }
                if (transform.position.y < 10)
                {
                    transform.position = new Vector2(transform.position.x, 10);
                }
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

                if (camera.orthographicSize < 6.6f)
                {
                    camera.orthographicSize = 6.6f;
                }
                if (camera.orthographicSize > 20)
                {
                    camera.orthographicSize = 20;
                }
            }


            if (Input.touchCount > 0 && !activeDialog)
            {
                touchDuration += Time.deltaTime;
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Ended && touchDuration < 0.1f)
                {
                    StartCoroutine("singleOrDouble");
                }
                else
                {
                    touchDuration = 0.0f;
                }
            }
        }
    }

    IEnumerator singleOrDouble()
    {
        yield return new WaitForSeconds(0.3f);
        if (touch.tapCount == 2)
        {
            /*
            StopCoroutine("singleOrDouble");
            if (camera.orthographicSize == 6.6f)
            {
                camera.orthographicSize = 20;
            }
            else
            {
                camera.orthographicSize = 6.6f;
            }
            */
        }
    }

    IEnumerator WaitForSmallTime()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject tempBuilding in allBuildings)
        {
            if (tempBuilding.GetComponent<CircleCollider2D>() != null)
            {
                tempBuilding.GetComponent<CircleCollider2D>().enabled = true;
                tempBuilding.GetComponent<BuildingMain>().canClick = true;
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
        if(direction.x > 1.5f || direction.x < -1.5f || direction.y > 1.5f || direction.y < -1.5f)
        {
            GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
            foreach (GameObject tempBuilding in allBuildings)
            {
                if (tempBuilding.GetComponent<CircleCollider2D>() != null)
                {
                    tempBuilding.GetComponent<CircleCollider2D>().enabled = false;
                    tempBuilding.GetComponent<BuildingMain>().canClick = false;
                }
            }
        }
        Vector3 position = camera_position + direction;
        transform.position = position;
    }
}
