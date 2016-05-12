using UnityEngine;
using System.Collections;
using System;

public class RotateSatellite : MonoBehaviour 
{
    private float baseAngle = 0.0f;
    Quaternion newRotation;

    void Start()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().material = new Material(Shader.Find("Particles/Additive"));
        GetComponent<LineRenderer>().SetColors(Color.red, Color.red);

        var direction = transform.forward;
        direction = new Vector3(0, direction.z, 0);
        var endPoint = transform.position + direction * 10;
        GetComponent<LineRenderer>().SetPosition(1, endPoint);
    }

    void Update()
    {
        if (newRotation != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 1);
        }
    }

    void OnMouseDown()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        baseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
    }

    void OnMouseDrag()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - baseAngle;
        newRotation = Quaternion.AngleAxis(ang, Vector3.forward);
    }
}
