using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    Slider camSensitivitySlider;
    float Xspeed,
        Yspeed,
        Zspeed;

    void LateUpdate()
    {
        Yspeed = Input.GetAxisRaw("Vertical") * camSensitivitySlider.value;
        Xspeed = Input.GetAxisRaw("Horizontal") * camSensitivitySlider.value;
        Zspeed = Input.mouseScrollDelta.y * camSensitivitySlider.value;

        transform.position = new Vector3(
            transform.position.x + Xspeed,
            transform.position.y + Yspeed,
            transform.position.z
        );
        Camera.main.orthographicSize -= Zspeed;
    }

    public void CenterCam(Vector3 pos)
    {
        transform.position = new(pos.x, pos.y, transform.position.z);
    }
}
