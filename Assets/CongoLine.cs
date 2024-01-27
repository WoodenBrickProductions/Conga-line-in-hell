using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongoLine : MonoBehaviour
{
    List<CongoScript> congos = new List<CongoScript>();

    private void Awake()
    {
        congos.AddRange(GetComponentsInChildren<CongoScript>());
    }

    private void OnDestroy()
    {
        congos.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(congos.Count != 0)
        {
            congos[0].SetLeader(true);
            congos[0].nextInLine = congos[0].transform;

            for(int i = 1; i < congos.Count; i++)
            {
                congos[i].SetLeader(false);
                congos[i].nextInLine = congos[i - 1].transform;
            }
        }
    }
}
