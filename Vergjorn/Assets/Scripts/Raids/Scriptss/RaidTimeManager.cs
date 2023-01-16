using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidTimeManager : MonoBehaviour
{
    public static RaidTimeManager Instance;

    public Light sun;
    [Range(0, 1)] public float currentTimeOfDay;

    public float timeMultiplier = 1f;
    public float sunStartIntensity;

    public float secondsInFullDay;

    public Color sunSetColor;
    public Color fullDayColor;

    public float timeToChangeColor = 5f;
    float t;
    [Range(0, 1)] public float startSunSetTime;
    bool sunsetColorActive;
    private void Awake()
    {
        Instance = this;


    }
    void Start()
    {
        sunStartIntensity = sun.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        if(currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }

        #region Change Colors

        if(currentTimeOfDay >= startSunSetTime && !sunsetColorActive)
        {
            if(t < timeToChangeColor)
            {
                t += Time.deltaTime;
                sun.color = Color.Lerp(fullDayColor, sunSetColor, t / timeToChangeColor);
            }
            else
            {
                sunsetColorActive = true;
            }
        }

        if (currentTimeOfDay <= 0.25f && sunsetColorActive)
        {
            
            if (t < timeToChangeColor)
            {
                t += Time.deltaTime;
                sun.color = Color.Lerp(fullDayColor, sunSetColor, t / timeToChangeColor);
            }
            else
            {
                sunsetColorActive = false;
                
            }
        }

        #endregion
    }

    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360) - 90, -125, 0);

        float intensityMultiplier = 1f;

        //First and last quater of day
        if(currentTimeOfDay <= 0.25f || currentTimeOfDay >= 0.75f)
        {
            intensityMultiplier = 0.25f;
        }else if(currentTimeOfDay <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if(currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.73f) * (1 / 0.02f));
        }

        intensityMultiplier = 1f;
        sun.intensity = sunStartIntensity * intensityMultiplier;
    }
}
