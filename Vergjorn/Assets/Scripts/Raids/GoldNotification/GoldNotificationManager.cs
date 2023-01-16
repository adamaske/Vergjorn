using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldNotificationManager : MonoBehaviour
{
    public static GoldNotificationManager Instance;

    public GameObject goldNotificationPrefab;

    public Transform canvas;
    private void Awake()
    {
        Instance = this;
    }

    public void GiveMeGoldNotification(GameObject unitPos, float amount, float size)
    {
        
        GameObject go = Instantiate(goldNotificationPrefab, Camera.main.WorldToScreenPoint(unitPos.transform.position), this.transform.rotation, this.transform);
        go.GetComponent<GoldNotification>().GetInfo(unitPos, amount, size);

    }
}
