using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var congo = other.GetComponent<CongoScript>();
        if(congo != null)
        {
            congo.Destroy();
        }
    }
}
