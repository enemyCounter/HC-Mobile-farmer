using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header(" Data ")]
    [SerializeField] private CropData[] _cropData;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public Sprite GetCropIcon(CropType cropType)
    {
        for(int i = 0;  i < _cropData.Length; i++)
        {
            if (_cropData[i].cropType == cropType)
                return _cropData[i].icon;
        }
        Debug.Log("No cropData found");
        return null;
    }
}
