using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaHidder : MonoBehaviour
{
    public Movable[] movables;

    private void Awake()
    {
        movables = GetComponentsInChildren<Movable>(true);

        for(int i = 0; i < movables.Length; i++)
        {
            movables[i].gameObject.SetActive(false);
            if (movables[i].gameObject.isStatic)
                continue;

            movables[i].transform.position += Vector3.down * 10;
        }
    }

    public Transform[] GetMovableTransforms()
    {
        Transform[] transforms = new Transform[movables.Length];
    
        for(int i = 0; i < transforms.Length; i++)
        {
            transforms[i] = movables[i].transform;
        }

        return transforms;
    }
}
