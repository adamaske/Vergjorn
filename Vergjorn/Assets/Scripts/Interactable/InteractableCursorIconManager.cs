using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class InteractableCursorIconManager : MonoBehaviour
{
    public bool alwaysCheck;

    public Image icon;

    public bool open;

    public Sprite chopTreeSprite;
    public Sprite buildSprite;
    public Sprite mineSprite;
    public Sprite fishSprite;

    Vector2 pos;
    private void Start()
    {
        DisableIcon();
    }

    private void Update()
    {
        if (alwaysCheck)
        {
            CheckHit();
        }

       
       
    }

    

    void CheckHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1000))
        {
            
            Tree tree = hit.collider.GetComponent<Tree>();
            if(tree != null)
            {
                icon.sprite = chopTreeSprite;
                if (!open)
                {
                    EnableIcon();
                }
                return;
            }

            Myrmalm myrmalm = hit.collider.GetComponent<Myrmalm>();
            if(myrmalm != null)
            {
                icon.sprite = mineSprite;
                if (!open)
                {
                    EnableIcon();
                }
                return;
            }
            BuildingStructures buildingStructures = hit.collider.GetComponent<BuildingStructures>();
            if(buildingStructures != null)
            {
                if(buildingStructures.unbuilt == true)
                {
                    icon.sprite = buildSprite;
                    if (!open)
                    {
                        EnableIcon();
                    }
                    return;
                }
            }
            FishingHut fishingHut = hit.collider.GetComponent<FishingHut>();
            if(fishingHut != null)
            {
                icon.sprite = fishSprite;
                if (!open)
                {
                    EnableIcon();
                }
                return;
            }

            DisableIcon();
        }
    }

    public void EnableIcon()
    {
        open = true;
        Cursor.visible = false;
        
        icon.gameObject.SetActive(true);
    }

    public void DisableIcon()
    {
        open = false;

        icon.gameObject.SetActive(false);

        Cursor.visible = true;
    }
}
