using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetImageSprite : MonoBehaviour
{
    public SpriteVariable sprite;
    
    void Start()
    {
        Image i = GetComponent<Image>();
        if(i != null)
        {
            i.sprite = sprite.asset;
        }
    }

}
