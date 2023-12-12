using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ActionTest.myAction += DisplayHealthPopup;
    }

    private void OnDestroy()
    {
        ActionTest.myAction -= DisplayHealthPopup;
    }

    private void DisplayHealthPopup(int health)
    {
        Debug.Log("Displaying Health Popup");
    }  
}
