using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySelectable : MonoBehaviour
{
    
    public Shader selectableShader;
    public Shader standardShader;

    
    Selectable currentSelectable;

    bool open;
    public LayerMask layerMask;

    public float outlineWidth;
    public Color outlineColor;
    
    
    private void Update()
    {
        

        HitSelecatble();
        standardShader = Shader.Find("Standard");
        
    }
    void HitSelecatble()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            Selectable selectable = hit.collider.GetComponent<Selectable>();
            if(selectable != null)
            {
                if(selectable == currentSelectable)
                {
                    //if hit == as last hit, do nothing
                }
                else
                {
                    //if hit new selectabel
                    ReturnMaterial();
                    currentSelectable = selectable;
                    
                    
                    
                    
                    EnableNewMaterial();
                    
                }
            }
            else
            {
                //if sele == null
                if(currentSelectable != null)
                {
                    ReturnMaterial();
                    currentSelectable = null;
                }
                                              
            }

        }
        else
        {
            //if sele == null
            if (currentSelectable != null)
            {
                ReturnMaterial();
                currentSelectable = null;
            }
        }
    }

    
    void EnableNewMaterial()
    {
        Renderer[] renderers= currentSelectable.GetComponentsInChildren<Renderer>();
        foreach(Renderer rend in renderers)
        {
            
                rend.material.shader = selectableShader;
           
            
        }
        
        open = true;
    }

    void ReturnMaterial()
    {
        if (currentSelectable != null)
        {
            //currentSelectable.GetComponentInChildren<Renderer>().material = returnMaterial;
            Renderer[] renderers = currentSelectable.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                
                    rend.material.shader = standardShader;
                
                
            }
            open = false;
        }
        
    }
}
