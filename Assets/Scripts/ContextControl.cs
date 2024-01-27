using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionContextAction
{
    PUSH_UP,
    PUSH_RIGHT,
    PUSH_DOWN,
    PUSH_LEFT,
}

public class SelectionContext
{
    public string name;
    public Dictionary<SelectionContextAction, Action<SelectionContextAction>> actions = 
        new Dictionary<SelectionContextAction, Action<SelectionContextAction>>();
}

public class ContextControl : MonoBehaviour
{
    private static ContextControl instance;

    public Transform selector;
    public ContextWheel contextWheel;
    private int interactableMask;

    private Interactable selected;
    private Interactable highlighted;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one context control in scene");
            return;
        }

        instance = this;
        interactableMask = LayerMask.GetMask("Interactable");

        contextWheel.OnPress += OnPress;
    }

    private void OnDestroy()
    {
        contextWheel.OnPress -= OnPress;
    }

    private void OnPress(SelectionContextAction dir)
    {
        selected.interactableObject.context.actions[dir].Invoke(dir);
        HideContextWheel();
        selected = null;
    }

    void Start()
    {
        HideContextWheel();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected != null)
            return;

        Highlight();

        if (Input.GetMouseButtonDown(0))
        {
            SelectCurrent();
        }
    }

    static Plane mousePlane = new Plane(Vector3.up, 0);

    public void SelectCurrent()
    {
        Highlight();

        selected = highlighted;

        if (selected != null)
        {
            selected.OnSelect();
            ShowContextWheel(selected.interactableObject.context);
        }
    }

    Interactable GetObjectUnderCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000, interactableMask))
        {
            var interactable = hitData.collider.GetComponent<Interactable>();
            return interactable;
        }

        return null;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (mousePlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    public void Highlight()
    {
        highlighted = GetObjectUnderCursor();
        if (highlighted == null)
        {
            selector.position = new Vector3(-1000, -1000, -1000);
            return;
        }
        
        selector.position = highlighted.transform.position;
    }

    public void ShowContextWheel(SelectionContext context)
    {
        contextWheel.SetupContext(context);
        contextWheel.gameObject.SetActive(true);
    }

    public void HideContextWheel()
    {
        contextWheel.ClearContext();
        contextWheel.gameObject.SetActive(false);
    }
}
