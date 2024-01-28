using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public bool startClosed = true;
    public float angleOpen;
    public float angleClosed;
    public Transform pivot;
    public Collider collider;
    private float currentAngle;

    public bool useExtraContext = false;
    public bool extraContextSingle = false;
    private bool extraContextUsed = false;
    public LeverActionContext extraContext;

    void Awake()
    {
        context.name = "Door";
        currentAngle = pivot.transform.eulerAngles.y;
        context.actions = new Dictionary<SelectionContextAction, System.Action<SelectionContextAction>>();
        context.actions.Add(SelectionContextAction.DOOR_OPEN, DoorFunc);
        context.actions.Add(SelectionContextAction.DOOR_CLOSE, DoorFunc);

        if(startClosed)
        {
            DoorFunc(SelectionContextAction.DOOR_CLOSE);
        }
        else
        {
            DoorFunc(SelectionContextAction.DOOR_OPEN);
        }
    }

    private void Update()
    {
        Vector3 angle = pivot.localEulerAngles;
        angle.y = Mathf.MoveTowards(angle.y, currentAngle, Mathf.Max(4, Mathf.Abs(angle.z - currentAngle)) * 50 * Time.deltaTime);
        pivot.localEulerAngles = angle;
    }

    public void DoorFunc(SelectionContextAction action)
    {
        switch (action)
        {
            case SelectionContextAction.DOOR_OPEN:
                currentAngle = angleOpen;
                collider.enabled = false;

                if (useExtraContext && !extraContextUsed)
                {
                    if (extraContextSingle)
                        extraContextUsed = true;

                    GlobalBehaviour.instance.StartCoroutine(Lever.MakeAppearCoroutine(extraContext));
                }
                break;
            case SelectionContextAction.DOOR_CLOSE:
                currentAngle = angleClosed;
                collider.enabled = true;
                break;
            default:
                break;
        }
    }

    public override void OnSelect()
    {
    }
}
