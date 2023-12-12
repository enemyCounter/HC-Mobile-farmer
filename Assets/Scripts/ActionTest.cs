using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : MonoBehaviour
{
    public static Action <int> myAction;


    // Start is called before the first frame update
    void Start()
    {
        myAction?.Invoke(7);
    }  
}
