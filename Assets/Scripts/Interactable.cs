using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private SphereCollider collidor;

    public InteractableObject interactableObject;

    // Start is called before the first frame update
    void Awake()
    {
        collidor = GetComponent<SphereCollider>();
        if (interactableObject == null)
            interactableObject = GetComponentInParent<InteractableObject>();
    }

    public void OnSelect()
    {
        interactableObject.OnSelect();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        collidor.enabled = active;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.DrawSphere(transform.position, collidor.radius);
    }
}
