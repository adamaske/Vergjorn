using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public float secondsPerDay = 60f;
    public float currentTime;

    [Range(0, 1)]
    public float startTimeOfDay;
    [Range(0, 1)]
    public float currentTimeOfDay;
    
    [System.Serializable]
    public class Month
    {
        [Tooltip("F.eks januar")]
        public string monthName;
        public bool isWinterMonth;
    }
    public Month[] months;
    public Month currentMonth;

    [System.Serializable]
    public class Day
    {
        [Tooltip("F.eks mandag")]
        public string dayName;
    }
    public Day[] days;
    public Day currentDay;

    public float daysInAWeek = 7f;
    public float weeksInAMonth = 4f;
    public float monthsInAYear = 12f;
    public bool timeGoing;

    public int monthIndex;
    public int dayIndex;
    public int weekIndex;

    [Header("Display")]
    public bool display;

    public TextMeshProUGUI monthName;
    public TextMeshProUGUI dayName;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentTime = secondsPerDay * startTimeOfDay;
    }

    public bool Winter()
    {
        if (currentMonth.isWinterMonth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Update()
    {
        if (timeGoing)
        {
            currentTimeOfDay = currentTime / secondsPerDay;


            if(currentTime < secondsPerDay)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                NextDay();
            }


        }

        if (display)
        {
            monthName.SetText(currentMonth.monthName);
            dayName.SetText(currentDay.dayName);
        }
    }
    public void LoadTime()
    {
        FileInfo info = new FileInfo(Application.persistentDataPath + "/saves/Time.save");
        if (!info.Exists)
        {
            //Reset to new 
            LoadNewGame();
            return;
        }

        TimeSave time = (TimeSave)SerializationManager.Load(Application.persistentDataPath + "/saves/Time.save");

        monthIndex = time.monthIndex;
        dayIndex = time.dayIndex;
        weekIndex = time.weekIndex;

        Set();

        timeGoing = true;
    }

    void Set()
    {

        currentMonth = months[monthIndex];
        currentDay = days[dayIndex];

    }
    public void LoadNewGame()
    {
        monthIndex = 0;
        dayIndex = 0;
        weekIndex = 0;

        Set();

        timeGoing = true;
    }
    public void SaveTime()
    {
        TimeSave t = new TimeSave();
        t.monthIndex = monthIndex;
        t.dayIndex = dayIndex;
        t.weekIndex = weekIndex;

        SerializationManager.Save("Time", t);
    }

    public void NextDay()
    {
        dayIndex += 1;
        if(dayIndex >= daysInAWeek)
        {
            dayIndex = 0;
            weekIndex += 1;
        }
        if(weekIndex >= weeksInAMonth)
        {
            weekIndex = 0;
            monthIndex += 1;
        }
        if(monthIndex >= monthsInAYear)
        {
            monthIndex = 0;
        }
        FoodTicker.Instance.NewDay();
        Set();
    }
}
[System.Serializable]
public class TimeSave
{
    public int monthIndex = 0;
    public int dayIndex = 0;
    public int weekIndex = 0;
}
