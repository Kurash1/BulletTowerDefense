using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float horizontal = 0;
    float vertical = 0;
    float zoom = 0;
    public float speed;
    public float zoomspeed;
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        zoom = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(new Vector2(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime));
        Camera.main.orthographicSize = Mathf.Max(1f, Camera.main.orthographicSize - (zoom * zoomspeed * Time.deltaTime));
    }
}
