 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SmieInteracter : MonoBehaviour
{
    public Smie currentSmie;
    public GameObject SmieInfo;


    public TextMeshProUGUI levelText;

 
    public TextMeshProUGUI upgradeWoodCostText;
    public TextMeshProUGUI upgradeMetalCostText;
    bool open;

    public Vector3 offset;

    public GameObject smieMenuGO;

    [Header("Queue stuff")]
    public List<Image> queueImages = new List<Image>();
    public Transform imageParent;


    [Header("In progress weapon")]
    public Image currentWeaponImage;
    public Image productionProgressBar;

    public static SmieInteracter Instance;

    [Header("Buttons")]
    public GameObject smieButtonPrefab;
    public Transform buttonParent;
    [Space]
    public Sprite swordSprite;
    public Sprite shieldSprite;
    public Sprite helmetSprite;


    public bool checkForDead;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        smieMenuGO.SetActive(false);

        foreach(Transform c in buttonParent)
        {
            Destroy(c.gameObject);
        }

        foreach(Transform child in imageParent)
        {
            Image i = child.GetComponent<Image>();
            if(i != null)
            {
                queueImages.Add(i);
            }
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           CheckSmieHit();
        }

        if (open)
        {
            SetInfo();

            SetQueue();
        }
    }
    void CheckSmieHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            Smie s = hit.collider.GetComponent<Smie>();
            if (s != null)
            {
                if (!MouseOverUI())
                {
                    if (!open)
                    {
                        if (checkForDead)
                        {
                            if(s.GetComponent<DestructableStructure>().isDead == true)
                            {
                                CloseSmieMenu();
                            }
                            else
                            {
                                OpenSmieMenu(s);
                            }
                        }
                        else
                        {
                            OpenSmieMenu(s);
                        }
                        
                    }
                }
               
                
            }
            else
            {
                if (!MouseOverUI())
                {
                    CloseSmieMenu();
                }

            }

        }
    }


    void OpenSmieMenu(Smie s)
    {
        currentSmie = s;
        smieMenuGO.SetActive(true);
        open = true;

        if (currentSmie.currentSmieLevel.canProduceHelmets)
        {
            GameObject go = Instantiate(smieButtonPrefab, buttonParent);
            SmieButton b = go.GetComponent<SmieButton>();

            b.GetSprite(helmetSprite);
            b.thisType = WeaponType.helmet;
            b.currentSmie = currentSmie;
        }
        if (currentSmie.currentSmieLevel.canProduceSwords)
        {
            GameObject go = Instantiate(smieButtonPrefab, buttonParent);
            SmieButton b = go.GetComponent<SmieButton>();

            b.GetSprite(swordSprite);
            b.thisType = WeaponType.sword;
            b.currentSmie = currentSmie;
        }
        if (currentSmie.currentSmieLevel.canProduceShield)
        {
            GameObject go = Instantiate(smieButtonPrefab, buttonParent);
            SmieButton b = go.GetComponent<SmieButton>();

            b.GetSprite(shieldSprite);
            b.thisType = WeaponType.shield;
            b.currentSmie = currentSmie;
            
        }
        

    }
    void CloseSmieMenu()
    {
        smieMenuGO.SetActive(false);
        open = false;

        foreach(Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ProduceWeapon(WeaponType t)
    {
        if(currentSmie.CanAffordWeaponGoldProduction(t) && currentSmie.CanAffordWeaponMetalProduction(t))
        {
            currentSmie.PurchaseWeaponProduction(t);
            currentSmie.ProduceWeapon(t);
        }
       
    }
    void SetInfo()
    {
      
        if (currentSmie.currentWeaponBeingMade != null)
        {
            if (currentSmie.currentWeaponBeingMade.myType == WeaponType.helmet)
            {
                currentWeaponImage.sprite = helmetSprite;
            }
            if (currentSmie.currentWeaponBeingMade.myType == WeaponType.sword)
            {
                currentWeaponImage.sprite = swordSprite;
            }
            if (currentSmie.currentWeaponBeingMade.myType == WeaponType.shield)
            {
                currentWeaponImage.sprite = shieldSprite;
            }
        }
        
        productionProgressBar.fillAmount = currentSmie.t / currentSmie.ProductionTime();
    }

    void SetQueue()
    {
        for (int i = 0; i < currentSmie.weaponQueue.Count; i++)
        {
            if(currentSmie.weaponQueue[i] != null)
            {
                queueImages[i].gameObject.SetActive(true);
                queueImages[i].sprite = GetSprite(currentSmie.weaponQueue[i].myType);
            }
            else
            {
                queueImages[i].gameObject.SetActive(false);

            }
        }
    }

    public Sprite GetSprite(WeaponType t)
    {
        if(t == WeaponType.helmet)
        {
            return helmetSprite;
        }
        if (t == WeaponType.sword)
        {
            return swordSprite;
        }
        if (t == WeaponType.shield)
        {
            return shieldSprite;
        }

        return null;
    }
    
    public void UpgradeSmie()
    {
        currentSmie.UpgradeSmie();
    }

    bool MouseOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


