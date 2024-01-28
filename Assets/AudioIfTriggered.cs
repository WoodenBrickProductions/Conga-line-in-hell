using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioIfTriggered : MonoBehaviour
{
    public bool singleUse;
    private bool used;
    public AudioSource audio;
    static event Action OnPlay;


    private void Awake()
    {
        used = false;
        OnPlay += AudioIfTriggered_OnPlay;
    }

    private void AudioIfTriggered_OnPlay()
    {
        audio.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        if (singleUse)
            used = true;

        var congo = other.GetComponentInParent<CongoScript>();
        if(congo != null)
        {
            OnPlay();
            audio.Play(); 
        }
    }
}

