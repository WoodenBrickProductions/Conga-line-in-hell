using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private bool cameraScroll = false;
    [SerializeField] private float CameraSpeed = 100;
    [SerializeField] private float panStrength = 0.5f;
    [SerializeField] private float slowDragDeadzone;
    [SerializeField] private float fastDragDeadzone;

    public Transform BLBound;
    public Transform TRBound;

    private Transform cameraOrigin;
    private Transform cameraHead;
    [SerializeField] private float accelerationSensitivity = 0.05f;
    private float hAcceleration;
    private float vAcceleration;

    public int minZoom = 45;
    public int maxZoom = 20;
    private int currentZoom = 0;

    private bool inFocus = false;

    private void Awake()
    {
        currentZoom = minZoom;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.focusChanged += Application_focusChanged;

        cameraHead = Camera.main.transform;
        cameraOrigin = Camera.main.transform.parent.parent;
    }

    private void OnDestroy()
    {
        Application.focusChanged -= Application_focusChanged;
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

        if(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.01f)
        {
            float amount = -10 * Input.GetAxis("Mouse ScrollWheel");
            currentZoom = Mathf.Clamp(currentZoom + (int)(amount),  maxZoom, minZoom);
        }

        Vector3 pos = cameraHead.transform.localPosition;
        pos.z = Mathf.MoveTowards(pos.z, -currentZoom, Mathf.Max(1, Mathf.Abs(currentZoom - pos.z)) * Time.deltaTime);
        cameraHead.transform.localPosition = pos;

        MoveCamera();
    }

    private void MoveCamera()
    {
        float horizontal = (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || (cameraScroll && Input.mousePosition.x < fastDragDeadzone) ? -1 * panStrength : 0) +
                         (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || (cameraScroll && Input.mousePosition.x > Screen.width - fastDragDeadzone) ? 1 * panStrength : 0);

        float vertical = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || (cameraScroll && Input.mousePosition.y > Screen.height - fastDragDeadzone) ? 1 * panStrength : 0) +
                       (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || (cameraScroll && Input.mousePosition.y < fastDragDeadzone) ? -1 * panStrength : 0);

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
        var nX = cameraOrigin.localPosition.x + x;
        if (nX > TRBound.localPosition.x ||
            nX < BLBound.localPosition.x)
            x = 0;

        float z = vAcceleration * CameraSpeed * Time.deltaTime;
        var nZ = cameraOrigin.localPosition.z + z;
        if (nZ > TRBound.localPosition.z ||
            nZ < BLBound.localPosition.z)
            z = 0;

        cameraOrigin.localPosition += new Vector3(x, 0, z);
    }
}
