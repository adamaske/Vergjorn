using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageNotification : MonoBehaviour
{
    public float lifespan = 1;

    public TextMeshProUGUI damageText;

    public Vector3 endOffset;

    public Vector3 offset = new Vector3();
    Vector3 currentOffset;
    bool moving;
    float t;


    GameObject assignedUnit;
    public float moveSpeed = 1;

    Vector3 startPos;
    public void GetInfo(GameObject unitPos, float amount, float size)
    {
        if (damageText != null)
        {
            damageText.text = "- " + amount.ToString();
        }
        transform.localScale *= size;

        Destroy(this.gameObject, lifespan);

        assignedUnit = unitPos;
        startPos = Camera.main.WorldToScreenPoint(assignedUnit.transform.position) + offset;

        moving = true;
    }


    private void Update()
    {
        if (assignedUnit != null)
        {
            startPos = Camera.main.WorldToScreenPoint(assignedUnit.transform.position) + offset;
        }
        transform.position = startPos;
        if (moving)
        {
            if (t < moveSpeed)
            {
                t += Time.deltaTime;
                currentOffset = Vector3.Lerp(Vector3.zero, endOffset, t / moveSpeed);
            }
            else
            {
                moving = false;
            }

        }
        transform.position += currentOffset;




    }
}
