using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNotificationManager : MonoBehaviour
{
    public static DamageNotificationManager Instance;

    public GameObject damageNotificationPrefab;

    public Transform canvas;
    private void Awake()
    {
        Instance = this;
    }

    public void GiveMeDamageNotification(GameObject unitPos, float amount, float size)
    {

        GameObject go = Instantiate(damageNotificationPrefab, Camera.main.WorldToScreenPoint(unitPos.transform.position), this.transform.rotation, this.transform);
        go.GetComponent<DamageNotification>().GetInfo(unitPos, amount, size);

    }
}
