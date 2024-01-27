using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionTrigger : InteractableObject
{
    public LeverActionContext actionContext;
    public Vector3 visionDirection;
    public float minDistance = 5;
    int wallMask;

    private float rayCooldown = 0.5f;
    private float rayTimer;

    private bool moving = false;
    private Vector3 targetPosition;
    void Awake()
    {
        wallMask = LayerMask.GetMask("Wall");
    }

    private void WallDown(SelectionContextAction action)
    {
        if (action != SelectionContextAction.WALL_DOWN_WITH_TRIGGER)
            return;

        Push(SelectionContextAction.PUSH_DOWN);
        GlobalBehaviour.instance.StartCoroutine(Lever.MakeAppearCoroutine(actionContext));
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (rayTimer > 0)
        {
            rayTimer -= Time.deltaTime;
            return;
        }

        rayTimer = rayCooldown;
        RaycastHit hit;
        float distance;
        if (Physics.Raycast(transform.position, visionDirection, out hit, Mathf.Infinity, wallMask))
        {
            distance = hit.distance;
        }
        else
        {
            distance = 1000;
        }

        if (distance < minDistance)
        {
            WallDown(SelectionContextAction.WALL_DOWN_WITH_TRIGGER);
        }
    }

    public void Push(SelectionContextAction action)
    {
        moving = true;
        switch (action)
        {
            case SelectionContextAction.PUSH_DOWN:
                targetPosition = transform.position + Vector3.down * 4;
                break;
            default:
                moving = false;
                break;
        }
    }

    public override void OnSelect()
    {
    }
}
