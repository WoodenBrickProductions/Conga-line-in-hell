using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeverAction
{
    MAKE_APPEAR,
    MAKE_DISAPPEAR,
    MAKE_MOVE,
    MAKE_SEND_ACTION,
}

[Serializable]
public struct LeverActionContext
{
    public LeverAction action;
    public InteractableObject target;
    public Transform moveTargetGroup;
    public bool cascade;
    public bool enable;
    public bool absolute;
    public Vector3 position;
    public float speed;
    public SelectionContextAction send_action;
}

public class Lever : InteractableObject
{
    public bool singleUse;
    private bool pulled;

    public LeverActionContext OnUnpulled;
    public LeverActionContext OnPulled;

    private void Awake()
    {
        pulled = false;
        context.name = "Lever";
        context.actions = new Dictionary<SelectionContextAction, System.Action<SelectionContextAction>>();
        context.actions.Add(SelectionContextAction.LEVER_PULL, LeverFunc);
    }

    public override SelectionContext GetContext()
    {
        if (singleUse && pulled)
            return null;

        return base.GetContext();
    }

    public void LeverFunc(SelectionContextAction action)
    {
        if (action != SelectionContextAction.LEVER_PULL)
            return;

        if (singleUse && pulled)
            return;

        pulled = !pulled;

        if(pulled)
        {
            LeverFunc(OnPulled);
        }
        else
        {
            LeverFunc(OnUnpulled);
        }

    }

    private void LeverFunc(LeverActionContext context)
    {
        switch (context.action)
        {
            case LeverAction.MAKE_APPEAR:
                MakeAppear(context);
                break;
            case LeverAction.MAKE_DISAPPEAR:
                MakeDissapear(context);
                break;
            case LeverAction.MAKE_MOVE:
                MakeMove(context);
                break;
            case LeverAction.MAKE_SEND_ACTION:
                MakeSendAction(context);
                break;
        }
    }

    private void MakeAppear(LeverActionContext context)
    {
        if(context.cascade)
        {
            StartCoroutine(MakeAppearCoroutine(context));
        }
        else
        {
            context.target.gameObject.SetActive(true);
            context.target.transform.position += context.position;
        }
    }

   

    private void MakeDissapear(LeverActionContext context)
    {
        context.target.gameObject.SetActive(false);
        context.target.transform.position -= context.position;
    }

    private void MakeMove(LeverActionContext context)
    {

    }

    private void MakeSendAction(LeverActionContext context)
    {
        var sendContext = context.target.GetContext();

        if (sendContext == null)
            return;

        sendContext.actions[context.send_action].Invoke(context.send_action);
    }

    public override void OnSelect()
    {
    }

    const float tick = 1.0f / 60;
    static WaitForSeconds wait = new WaitForSeconds(tick);
    public static IEnumerator MakeAppearCoroutine(LeverActionContext context)
    {
        List<Transform> children = new List<Transform>();
        int maxNext;

        var areaHidder = context.moveTargetGroup.GetComponent<AreaHidder>();
        if (areaHidder != null)
        {
            children.AddRange(areaHidder.GetMovableTransforms());
            maxNext = children.Count;
        }
        else
        {
            maxNext = context.moveTargetGroup.childCount;
            for (int i = 0; i < maxNext; i++)
            {
                children.Add(context.moveTargetGroup.GetChild(i));
            }
        }

        float wholeTime = 1;
        float speed = 1.5f * context.position.magnitude / wholeTime;

        float nextTimer = 0;
        int currentNext = 1;

        float nextCooldown = 0.3f / maxNext;

        Vector3[] targetPositions = new Vector3[maxNext];
        for (int i = 0; i < maxNext; i++)
        {
            targetPositions[i] = children[i].transform.position + context.position;
        }

        if (children.Count == 0)
            yield break;

        wholeTime = wholeTime * 2;
        children[0].gameObject.SetActive(true);
        while (wholeTime > 0)
        {
            for (int i = 0; i < maxNext; i++)
            {
                if (i >= currentNext)
                {
                    break;
                }

                Vector3 position = children[i].transform.position;
                children[i].transform.position = Vector3.MoveTowards(position, targetPositions[i], speed * tick);
            }

            if (nextTimer < 0 && (currentNext < maxNext))
            {
                int countActivated = Mathf.Max(1, Mathf.Abs((int)(nextTimer / nextCooldown)));
                nextTimer = nextCooldown;

                for (int i = 0; i < countActivated; i++)
                {
                    if (currentNext >= maxNext)
                        break;

                    children[currentNext++].gameObject.SetActive(true);
                }
            }

            nextTimer -= tick;
            wholeTime -= tick;
            yield return wait;
        }

        yield return 0;
    }
}
