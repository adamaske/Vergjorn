using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColorLerp : MonoBehaviour
{
    public Color day;
    public Color night;

    public Color currentColor;

    public TimeManager timeManager;

    float secondsPerDay;
    float t;

    public Light lightC;
    private void Start()
    {
        t = 0;
        secondsPerDay = timeManager.secondsPerDay;
    }

    private void Update()
    {
        lightC.color = Color.Lerp(day, night, t);
        if(t < 1)
        {
            t += Time.deltaTime / secondsPerDay;
        }
    }


}
