using UnityEngine;
using System.Collections;
using ProjectAlice;

public class ObjectControlled : MonoBehaviour
{
    //bool isRed = false;
    public int wiiID;

    void OnEnable()
    {
        AliceInstanceManager.instance.OnButtonRelease += OnButtonRelease;
        AliceInstanceManager.instance.OnButtonPress += OnButtonPressed;
    }

    void OnDisable()
    {
        AliceInstanceManager.instance.OnButtonRelease -= OnButtonRelease;
        AliceInstanceManager.instance.OnButtonPress -= OnButtonPressed;
    }

    void OnButtonRelease(ButtonEvent be)
    {
        if(be.ControllerID == wiiID && be.button == WiiButtonCode.B)
            GetComponent<Renderer>().material.color = Color.green;
    }

    void OnButtonPressed(ButtonEvent be)
    {
        Debug.Log(be.button);
        if(be.ControllerID == wiiID && be.button == WiiButtonCode.B)
            GetComponent<Renderer>().material.color = Color.red;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
        }
        
        if(Input.GetKeyDown(KeyCode.N))
        {
            AliceInstanceManager.instance.RumbleController(wiiID);
        }
    }
}
