using UnityEngine;
using System;


public class CropTile : MonoBehaviour
{
    private TileFieldState _state;

    [Header(" Elements ")]
    [SerializeField] private Transform _cropParent;
    [SerializeField] private MeshRenderer _tileRenderer;
    private Crop _crop;
    private CropData _cropData;

    [Header(" Elements ")]
    public static Action<CropType> onCropHarvested;


    public void Sow(CropData cropData)
    {
        _state = TileFieldState.Sown;

        _crop = Instantiate(cropData.cropPrefab, transform.position, Quaternion.identity, _cropParent);

        this._cropData = cropData;
    }

    public void Water()
    {
        _state = TileFieldState.Watered;      

        _crop.ScaleUp();
        // LeanTween CropTile color fade
        _tileRenderer.gameObject.LeanColor(Color.white * 0.5f, 1);     
    }

    public void Harvest()
    {
        _state = TileFieldState.Empty;

        _crop.ScaleDown();

        _tileRenderer.gameObject.LeanColor(Color.white, 1);

        onCropHarvested?.Invoke(_cropData.cropType);
    }

    public bool IsEmpty()
    {
        return _state == TileFieldState.Empty;
    }

    public bool IsSown()
    {
        return _state == TileFieldState.Sown;
    }
}
