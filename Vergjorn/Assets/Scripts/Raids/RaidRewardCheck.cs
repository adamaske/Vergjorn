using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RaidRewardCheck : MonoBehaviour
{
    public RaidReward reward;

    public FloatVariable gold;

    [Header("Notification")]
    public bool useNotification;
    public GameObject notificationObject;
    public TextMeshProUGUI goldText;
    public void Start()
    {
        CloseNotification();
        CheckForRewards();
        
    }

    public void CheckForRewards()
    {
        float goldReward = reward.goldReward;
        Debug.Log("Check reward");
        if(goldReward != 0)
        {
            OpenNotification();
            gold.value += goldReward;
        }

        ClearReward();
    }

    public void ClearReward()
    {
        reward.goldReward = 0;
    }

    void OpenNotification()
    {
        notificationObject.SetActive(true);
        goldText.text = "You looted " + reward.goldReward.ToString() + " gold!";
    }

    public void CloseNotification()
    {
        notificationObject.SetActive(false);
    }
}
