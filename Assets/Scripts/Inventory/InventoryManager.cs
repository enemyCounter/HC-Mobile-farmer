using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    private Inventory _inventory;
    private InventoryDisplay _inventoryDisplay;
    private string _dataPath;

    // Start is called before the first frame update
    void Start()
    {
        _dataPath = Application.dataPath + "/inventoryData.txt";

        LoadInventory();
        ConfigureInventoryDisplay();

        CropTile.onCropHarvested += CropHarvestedCallback;
    }

    private void OnDestroy()
    {
        CropTile.onCropHarvested -= CropHarvestedCallback;
    }

    private void ConfigureInventoryDisplay()
    {
        _inventoryDisplay = GetComponent<InventoryDisplay>();
        _inventoryDisplay.Configure(_inventory);
    }

    private void CropHarvestedCallback(CropType cropType)
    {
        _inventory.CropHarvestCallback(cropType);
        SaveInventory();
    }

    private void LoadInventory()
    {
        string data = "";

        if (File.Exists(_dataPath))
        {
           data = File.ReadAllText(_dataPath);
           _inventory = JsonUtility.FromJson<Inventory>(data);

            if( _inventory == null )            
                _inventory = new Inventory();            
        }

        else
        {
            File.Create(_dataPath);
            _inventory = new Inventory();
        }

    }

    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(_inventory, true);
        File.WriteAllText(_dataPath, data);
    }
}
