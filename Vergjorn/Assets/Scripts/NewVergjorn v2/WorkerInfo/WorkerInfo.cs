using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WorkerInfo : MonoBehaviour
{
    public static WorkerInfo Instance;
    public GameObject infoTab;

    public Worker currentWorker;

    public bool useClick;
    public bool alwaysCheck;

    public bool menuOpen;
    [Space]
    [Header("Text meshes")]
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI typeText;

    public TextMeshProUGUI levelText;

    public TextMeshProUGUI combatLevelText;
    public Image swordImage;
    public Image shieldImage;
    public Image helmetImage;

    public Sprite noWeaponSprite;
    public Sprite swordSprite;
    public Sprite shieldSprite;
    public Sprite helmetSprite;

    [Header("Experience stuff")]
    public Image combatXPBar;
    public Image workerXPBar;

    public LayerMask workerLayer;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CloseMenu();
    }

    private void Update()
    {
        if (alwaysCheck)
        {
            CheckHit();
        }

        if (useClick)
        {
            if (Input.GetMouseButtonDown(0) && !PauseMenu.Instance.Paused())
            {
                CheckHit();
            }
        }

        if (menuOpen)
        {
           
            SetInfo();
        }
    }
    void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000, workerLayer))
        {
            Worker worker = hit.collider.GetComponent<Worker>();
            if(worker != null)
            {
                currentWorker = worker;
                if (!menuOpen)
                {
                    OpenMenu();
                }
            }
            else
            {
                if (menuOpen)
                {
                    CloseMenu();
                }
            }
        }
        else
        {
            if (menuOpen)
            {
                CloseMenu();
            }
        }
    }

    public void OpenMenu()
    {
        infoTab.SetActive(true);
        menuOpen = true;
    }

    public void GetWorker(Worker worker)
    {
        currentWorker = worker;
        if (!menuOpen)
        {
            OpenMenu();
        }
    }
    void SetInfo()
    {
        nameText.SetText(currentWorker.myName.nameString);

        typeText.SetText(currentWorker.workerType.ToString());

        if (currentWorker.myData.hasShield)
        {
            shieldImage.sprite = shieldSprite;
        }
        else
        {
            shieldImage.sprite = noWeaponSprite;
        }
        if (currentWorker.myData.hasSword)
        {
            swordImage.sprite = swordSprite;
        }
        else
        {
            swordImage.sprite = noWeaponSprite;
        }
        if (currentWorker.myData.hasHelmet)
        {
            helmetImage.sprite = helmetSprite;
        }
        else
        {
            helmetImage.sprite = noWeaponSprite;
        }

        levelText.text = ("Worker level: " + currentWorker.currentWorkerLevel.levelName);

        workerXPBar.fillAmount = currentWorker.currentXP / currentWorker.currentWorkerLevel.xpForNextLevel;
        combatXPBar.fillAmount = currentWorker.myData.combatXP / currentWorker.myData.goalCombatXP;

        combatLevelText.SetText("Combat level: " + currentWorker.currentCombatLevel.levelName);
    }

    public void CloseMenu()
    {
        infoTab.SetActive(false);
        menuOpen = false;
        currentWorker = null;
    }
}
