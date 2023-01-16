using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffUI : MonoBehaviour
{
    public GameObject ui;

    public bool active;

    public float xRot;
    public float rotSens;
    public Transform cam;

    private void Start()
    {
        xRot = cam.rotation.x;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (active)
            {
                DisableUI();
            }
            else
            {
                Enable();
            }
        }

        if (!active)
        {
            float r = Input.GetAxis("Horizontal") * rotSens;

            xRot += r * Time.deltaTime;

            cam.transform.Rotate(r, 0, 0);
        }
        
    }

    public void DisableUI()
    {
        ui.SetActive(false);
        active = false;
    }
    public void Enable()
    {
        ui.SetActive(true);
        active = true;
    }
}
