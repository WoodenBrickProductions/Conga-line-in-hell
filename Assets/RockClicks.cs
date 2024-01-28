using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockClicks : MonoBehaviour
{
    public AudioSource source1;
    public AudioSource source2;
    public AudioSource source3;

    public float duration;
    public float waitBetween = 0.1f;
    float waitTimer = 0;

    // Update is called once per frame
    void Update()
    {
        if (duration < 0)
        {
            return;
        }
        duration -= Time.deltaTime;

        if(waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }
        waitTimer = waitBetween;

        int rand = Random.Range(0, 2);

        switch(rand)
        {
            case 0:
                source1.Play();
                break;
            case 1:
                source2.Play();
                break;
            case 2:
                source3.Play();
                break;
        }
    }
}
