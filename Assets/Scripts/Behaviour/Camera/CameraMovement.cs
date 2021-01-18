using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 absoluteAngleLimits;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Screen.safeArea.Contains(Input.mousePosition))
            return;

        Vector3 centeredMousePos = Input.mousePosition;
        centeredMousePos.x -= Screen.width/2;
        centeredMousePos.y -= Screen.height/2;

        var euler = transform.rotation.eulerAngles;
        euler.x = -Mathf.LerpUnclamped(0, absoluteAngleLimits.x, centeredMousePos.y / Screen.height);
        euler.y = Mathf.LerpUnclamped(0, absoluteAngleLimits.y, centeredMousePos.x / Screen.width);
        transform.rotation = Quaternion.Euler(euler);
    }
}
