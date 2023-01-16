using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableIndicator : MonoBehaviour
{
    public InteractableIndicatorManager manager;

    private void Update()
    {
        if(manager.hoveredObject != null)
        {
            Renderer r = manager.hoveredObject.GetComponentInChildren<Renderer>();
            Bounds bigBounds = r.bounds;

            Renderer[] rs = manager.hoveredObject.GetComponentsInChildren<Renderer>();
            foreach(Renderer rk in rs)
            {
                if(rk != r && rk.gameObject.name != "Quad")
                {
                    bigBounds.Encapsulate(rk.bounds);
                }
            }

            this.transform.position = new Vector3(bigBounds.center.x, 0, bigBounds.center.z);
            this.transform.localScale = new Vector3(bigBounds.size.x * 1.1f, 1, bigBounds.size.z * 1.1f);
        }
    }
}
