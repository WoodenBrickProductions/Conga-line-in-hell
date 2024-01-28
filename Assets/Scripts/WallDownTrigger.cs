using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDownTrigger : InteractableObject
{
    public LeverActionContext actionContext;

    private bool moving = false;
    private Vector3 targetPosition;

    void Awake()
    {
        context.name = "Push wall";
        context.actions = new Dictionary<SelectionContextAction, System.Action<SelectionContextAction>>();
        context.actions.Add(SelectionContextAction.WALL_DOWN_WITH_TRIGGER, WallDown);
    }

    private void WallDown(SelectionContextAction action)
    {
        if (action != SelectionContextAction.WALL_DOWN_WITH_TRIGGER)
            return;

        GetComponentInChildren<Interactable>().SetActive(false);
        Push(SelectionContextAction.PUSH_DOWN);
        GlobalBehaviour.instance.StartCoroutine(Lever.MakeAppearCoroutine(actionContext));
    }

    private void Update()
    {
        if (!moving)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 3 * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            moving = false;
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
        
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public override void OnSelect()
    {
    }
}
