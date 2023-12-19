using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private Transform _cropContainerParent;
    [SerializeField] private UICropContainer _uiCropContainerPrefab;


    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer cropContainerInstance = Instantiate(_uiCropContainerPrefab, _cropContainerParent);

            Sprite cropIcon = DataManager.instance.GetCropIcon(items[i].cropType);

            cropContainerInstance.Configure(cropIcon, items[i].amount);
        }
    }

    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        // Clear the crop containers parent
        while(_cropContainerParent.childCount > 0)
        {
            Transform container = _cropContainerParent.GetChild(0);
            container.SetParent(null);
            Destroy(container.gameObject);
        }
       
        // Create the UI containers from scratch again
        Configure(inventory);

        for(int i = 0;i < items.Length;i++)
        {
        UICropContainer cropContainerInstance = Instantiate(_uiCropContainerPrefab, _cropContainerParent);

            Sprite cropIcon = DataManager.instance.GetCropIcon(items[i].cropType);

            cropContainerInstance.Configure(cropIcon, items[i].amount);
        }
    }
}
