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
}
