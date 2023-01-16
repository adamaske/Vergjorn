using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RaidStructureGraphic : MonoBehaviour
{
    public RaidTask myTask;

    public Image healthBar;

    public GameObject deadGraphic;

    public Image iconImage;

    bool setDead;

    private void Update()
    {
        if (!setDead)
        {
            SetInfo();
            if (!setDead)
            {
                if (myTask.dead)
                {
                    deadGraphic.SetActive(true);
                    setDead = true;
                }
            }
        }
        
    }
    public void GetRaidTask(RaidTask task)
    {
        myTask = task;
        healthBar.fillAmount = myTask.health / myTask.maxHealth;
        iconImage.sprite = myTask.mySprite;

        deadGraphic.SetActive(myTask.dead);
        setDead = myTask.dead;
    }

    void SetInfo()
    {
        healthBar.fillAmount = myTask.health / myTask.maxHealth;

        
    }
   
}
