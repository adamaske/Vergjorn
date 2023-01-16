using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureGraphic : MonoBehaviour
{
    public GameObject thisGraphic;
    public GameObject constructionGraphic;

    public void UnbuiltGraphicActive()
    {
        if(constructionGraphic != null)
        {
            thisGraphic.SetActive(false);
            constructionGraphic.SetActive(true);
        }
            
        
        
    }
    public void BuiltGraphicActivate()
    {
       if(constructionGraphic != null)
        {
            thisGraphic.SetActive(true);
            constructionGraphic.SetActive(false);
        }
            
        
        
    }
}
