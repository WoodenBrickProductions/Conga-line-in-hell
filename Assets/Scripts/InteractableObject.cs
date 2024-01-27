using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public SelectionContext context = new SelectionContext();
    public SelectionContext GetContext()
    {
        return context;
    }

    public abstract void OnSelect();

}
