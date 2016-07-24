using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour 
{

    void LateUpdate()
    {
        float scrolling = Input.GetAxis("Mouse ScrollWheel");

        float orthSize = Camera.main.orthographicSize;

        if (scrolling > 0f && Camera.main.orthographicSize >= 230 && Camera.main.orthographicSize <= 730)
        {
            orthSize -= 30;
        }
        else if (scrolling < 0f && Camera.main.orthographicSize >= 230 && Camera.main.orthographicSize <= 730)
        {
            orthSize += 30;
        }

        Camera.main.orthographicSize = Mathf.Clamp(orthSize, 230, 730);
    }
}
