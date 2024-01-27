using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected SelectionContext context = new SelectionContext();
    public virtual SelectionContext GetContext()
    {
        return context;
    }

    public abstract void OnSelect();

}
