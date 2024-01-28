using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongLever : MonoBehaviour
{
    public bool singleUse = false;
    bool used = false;

    private void Awake()
    {
        used = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        if (singleUse)
            used = true;

        GetComponent<AudioSource>().Play();
    }
}
