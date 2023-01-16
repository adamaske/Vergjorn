using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightLighting : MonoBehaviour
{
    public Transform lightParent;

   
    public float timeToChangeColor = 5f;
    float t;
   
    public float normalSunIntensity;
    public float normalmMoonIntensity;

    bool sunsetColorActive;
    public Light sun;
    public Light moon;

    public float intensityDecreaseTime;
    float intensityT;

    [Range(0, 1)]public float dayStart;
    [Range(0, 1)] public float dayEnd;
    [Range(0, 1)] public float nightStart;
    [Range(0, 1)] public float nightEnd;

    public bool day;
    public bool night;

    public Color dayColor;
    public Color nightColor;

    public bool changeOnStart;
    private void Update()
    {
       
        UpdateSun();
    }
    void UpdateSun()
    {
        float currentTime = TimeManager.Instance.currentTimeOfDay;

        if (currentTime > dayStart && currentTime < dayEnd)
        {
            if(day == false)
            {
                day = true;
                if (changeOnStart)
                {
                    StartCoroutine(TurnOffLight(sun, 0, 0, nightColor, dayColor));
                }
                else
                {
                    changeOnStart = true;
                }
                
                //StartCoroutine(TurnOffLight(moon, 1, 0));
                //StartCoroutine(TurnOffLight(sun, 0, 1));
            }           
        }
        else
        {
            day = false;
        }

        if(currentTime > nightStart && currentTime < nightEnd)
        {
            if(night == false)
            {
                night = true;
                StartCoroutine(TurnOffLight(sun, 0, 0, dayColor, nightColor));
                //StartCoroutine(TurnOffLight(sun, 1, 0));
                //StartCoroutine(TurnOffLight(moon, 0, 1));
            }
        }
        else
        {
            night = false;
        }

        /*
        lightParent.transform.localRotation = Quaternion.Euler((currentTime * 360) - 90, -90, 0);


        if (currentTime > startSunSetTime && currentTime < endSunSetTime &&!sunsetColorActive)
        {
            Debug.Log("Sun set");
            if (t < timeToChangeColor)
            {
                t += Time.deltaTime;
                sun.color = Color.Lerp(fullDayColor, sunSetColor, t / timeToChangeColor);
               
            }
            else
            {
                sunsetColorActive = true;
                t = 0;
            }
        }

        if (currentTime > sunRiseStartTime && currentTime < sunRiseEndTime && sunsetColorActive)
        {
            Debug.Log("Sun rise");
            sun.color = fullDayColor;
            if (t < timeToChangeColor)
            {
                t += Time.deltaTime;
                sun.color = fullDayColor;
                //sun.color = Color.Lerp(sunSetColor, fullDayColor, t / timeToChangeColor);
               
            }
            else
            {
                sunsetColorActive = false;
                t = 0;

            }
        }
        */
        
    }

    IEnumerator TurnOffLight(Light l, float from, float to, Color fromC, Color toC)
    {
        Debug.Log("Started");
        intensityT = 0;
        yield return null;

       
        while(intensityT < intensityDecreaseTime)
        {
            intensityT += Time.deltaTime;

            l.color = Color.Lerp(fromC, toC, intensityT / intensityDecreaseTime);
            yield return null;
        }

        //while(intensityT < intensityDecreaseTime)
        //{
        //    intensityT += Time.deltaTime;

        //    l.intensity = Mathf.Lerp(from, to, intensityT / intensityDecreaseTime);

        //    yield return null;
        //}


        yield return null;
    }
    
}
