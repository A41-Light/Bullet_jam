using UnityEngine;

public class Camera_zoom_control : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    float targetAspect = 16f / 9f; // your design resolution aspect ratio
    float windowAspect = (float)Screen.width / Screen.height;
    float scaleFactor = targetAspect / windowAspect;

    Camera.main.orthographicSize = 7.5f * scaleFactor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
