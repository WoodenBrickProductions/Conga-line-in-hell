using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBehaviour : MonoBehaviour
{
    public static GlobalBehaviour instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}