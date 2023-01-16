using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawScreenRectangle : MonoBehaviour
{

    public GUIStyle mouseDragSkin;

    private Vector3 mouseDownPoint;
    private Vector3 mouseUpPoint;
    private Vector3 currentMousePoint;

    public int mouseButtonID = 0;
    private bool isClicking = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(mouseButtonID) && !PauseMenu.Instance.Paused())
        {
            StartDrawing(GetMouseWorldPos());
        }

        if (isClicking && Input.GetMouseButtonUp(mouseButtonID) )
        {
            EndDraw();
        }
    }

    public void StartDrawing(Vector3 mouseStartPos)
    {
        isClicking = true;
        mouseDownPoint = mouseStartPos;
    }

    public void EndDraw()
    {
        isClicking = false;
    }

    private void OnGUI()
    {
        if (isClicking)
        {
            float boxWidth = Camera.main.WorldToScreenPoint(mouseDownPoint).x - Camera.main.WorldToScreenPoint(GetMouseWorldPos()).x;
            float boxHeight = Camera.main.WorldToScreenPoint(mouseDownPoint).y - Camera.main.WorldToScreenPoint(GetMouseWorldPos()).y; ;

            
            float boxLeft = Input.mousePosition.x;
            float boxTop = (Screen.height - Input.mousePosition.y) - boxHeight;

            GUI.Box(new Rect(boxLeft, boxTop, boxWidth, boxHeight), "", mouseDragSkin);
        }
        //box width, height, top, left



    }
    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
