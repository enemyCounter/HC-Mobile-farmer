using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ActionTest.myAction += PlayTakeDamageSound;
    }

    private void OnDestroy()
    {
        ActionTest.myAction -= PlayTakeDamageSound;
    }

    private void PlayTakeDamageSound(int health)
    {
        Debug.Log("Playing taking damage sound");
    }
}
