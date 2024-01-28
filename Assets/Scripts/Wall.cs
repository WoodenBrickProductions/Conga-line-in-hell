using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : InteractableObject
{
    Vector3 targetPosition;
    bool moving = false;

    void Awake()
    {
        context.name = "Push wall";
        context.actions = new Dictionary<SelectionContextAction, System.Action<SelectionContextAction>>();
        context.actions.Add(SelectionContextAction.PUSH_UP, Push);
        context.actions.Add(SelectionContextAction.PUSH_RIGHT, Push);
        context.actions.Add(SelectionContextAction.PUSH_DOWN, Push);
        context.actions.Add(SelectionContextAction.PUSH_LEFT, Push);

    }

    private void Update()
    {
        if (!moving)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
        
        if(transform.position == targetPosition)
        {
            moving = false;
        }
    }

    public void Push(SelectionContextAction action)
    {
        moving = true;
        switch (action)
        {
            case SelectionContextAction.PUSH_UP:
                targetPosition = transform.position + Vector3.forward;
                break;
            case SelectionContextAction.PUSH_RIGHT:
                targetPosition = transform.position + Vector3.right;
                break;
            case SelectionContextAction.PUSH_DOWN:
                targetPosition = transform.position + Vector3.back;
                break;
            case SelectionContextAction.PUSH_LEFT:
                targetPosition = transform.position + Vector3.left;
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
