using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private bool cameraScroll = true;
    [SerializeField] private float CameraSpeed = 100;
    [SerializeField] private float slowDragDeadzone;
    [SerializeField] private float fastDragDeadzone;

    public Transform BLBound;
    public Transform TRBound;

    private Transform cameraTM;
    [SerializeField] private float accelerationSensitivity = 0.05f;
    private float hAcceleration;
    private float vAcceleration;

    private bool inFocus = false;

    // Start is called before the first frame update
    void Start()
    {
        Application.focusChanged += Application_focusChanged;

        cameraTM = Camera.main.transform.parent;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Application_focusChanged(bool obj)
    {
        ChangeFocus(obj);
    }

    private void ChangeFocus(bool focus)
    {
        inFocus = focus;
        if(inFocus)
        {
            inFocus = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            cameraScroll = true;
        }
        else
        {
            inFocus = false;
            Cursor.lockState = CursorLockMode.None;
            cameraScroll = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeFocus(false);
        }

        MoveCamera();
    }

    private void MoveCamera()
    {
        int horizontal = (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || (cameraScroll && Input.mousePosition.x < fastDragDeadzone) ? -1 : 0) +
                         (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || (cameraScroll && Input.mousePosition.x > Screen.width - fastDragDeadzone) ? 1 : 0);

        int vertical = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || (cameraScroll && Input.mousePosition.y > Screen.height - fastDragDeadzone) ? 1 : 0) +
                       (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || (cameraScroll && Input.mousePosition.y < fastDragDeadzone) ? -1 : 0);

        if(horizontal != 0)
        {
            hAcceleration = Mathf.Clamp(hAcceleration + horizontal * 0.1f, -3, 3);
        }
        else
        {
            hAcceleration = Mathf.MoveTowards(hAcceleration, 0, accelerationSensitivity * 2); 
        }

        if (vertical != 0)
        {
            vAcceleration = Mathf.Clamp(vAcceleration + vertical * 0.1f, -3, 3);
        }
        else
        {
            vAcceleration = Mathf.MoveTowards(vAcceleration, 0, accelerationSensitivity * 2);
        }

        float x = hAcceleration * CameraSpeed * Time.deltaTime;
        var nX = cameraTM.position.x + x;
        if (nX > TRBound.position.x ||
            nX < BLBound.position.x)
            x = 0;

        float z = vAcceleration * CameraSpeed * Time.deltaTime;
        var nZ = cameraTM.position.z + z;
        if (nZ > TRBound.position.z ||
            nZ < BLBound.position.z)
            z = 0;

        cameraTM.position += new Vector3(x, 0, z);
    }
}
