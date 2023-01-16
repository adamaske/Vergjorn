using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RaidHealthProgressDisplay : MonoBehaviour
{
    public Image bar;


    public TextMeshProUGUI progressText;


    private void Update()
    {
        float current = RaidManager.Instance.CurrentRaidHealth();
        float max = RaidManager.Instance.MaxRaidHealth();
        bar.fillAmount = current / max;

        progressText.text = current.ToString() + " / " + max.ToString();
    }
}
