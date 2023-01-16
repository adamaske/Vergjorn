using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class NotificationManager : MonoBehaviour
{
    public TextMeshProUGUI notificationName;
    public TextMeshProUGUI notificationText;

    public float lifeTime;

    bool notificationOpen;

    float t;

    public Transform notificationObject;

    public static NotificationManager Instance;

    public Image notificatonSprite;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CloseNotification();
    }
    private void Update()
    {
        if (notificationOpen)
        {
            if(t < lifeTime)
            {
                t += Time.deltaTime;
            }
            else
            {
                CloseNotification();
            }
        }
    }
    public void OpenNotification(string notName, string notText, float currentLifeTime, Sprite icon)
    {
        notificationName.text = notName;
        notificationText.text = notText;
        lifeTime = currentLifeTime;
        notificationObject.gameObject.SetActive(true);
        notificationOpen = true;
        notificatonSprite.sprite = icon;
        t = 0;
    }

    public void CloseNotification()
    {
        notificationOpen = false;
        notificationObject.gameObject.SetActive(false);
    }
}
