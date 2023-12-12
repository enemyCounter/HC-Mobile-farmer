using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform _tileParent;
    private List<CropTile> _cropTiles = new List<CropTile>();

    [Header(" Settings ")]
    [SerializeField] private CropData _cropData;
    private TileFieldState _fieldState;

    private int _tilesSown;
    private int _tilesWatered;
    private int _tilesHarvested;

    [Header(" Actions ")]
    public static Action<CropField> onFullySown;
    public static Action<CropField> onFullyWatered;
    public static Action<CropField> onFullyHarvested;

    // Start is called before the first frame update
    void Start()
    {
        _fieldState = TileFieldState.Empty;
        StoreTiles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StoreTiles()
    {
        for (int i = 0; i < _tileParent.childCount; i++)
        {
            _cropTiles.Add(_tileParent.GetChild(i).GetComponent<CropTile>());
        }
    }

    public void SeedCollidedCallback(Vector3[] seedPositions)
    {
        for (int i = 0;  i < seedPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosesCropTile(seedPositions[i]);

            //Debug.Log(closestCropTile.name);

            if (closestCropTile == null)
                continue;

            if (!closestCropTile.IsEmpty())
                continue;

            Sow(closestCropTile);
        }
    }

    private void Sow(CropTile cropTile)
    {
        cropTile.Sow(_cropData);
        _tilesSown++;

        if (_tilesSown >= _cropTiles.Count)
            FieldFullySown();
    }

    public void WaterCollidedCallback(Vector3[] waterPositions)
    {
        for (int i = 0; i < waterPositions.Length; i++)
        {
            CropTile closestCropTile = GetClosesCropTile(waterPositions[i]);

            if (closestCropTile == null)
                continue;

            if (!closestCropTile.IsSown())
                continue;

            Water(closestCropTile);
        }
    }

    private void Water(CropTile cropTile)
    {
        cropTile.Water();
        _tilesWatered++;

        if(_tilesWatered >= _cropTiles.Count)
        {
            FieldFullyWatered();
        }
    }

    private void FieldFullySown()
    {
        Debug.Log("field fully sown");
        _fieldState = TileFieldState.Sown;

        onFullySown.Invoke(this);
    }

    private void FieldFullyWatered()
    {
        Debug.Log("Field fully watered");

        _fieldState = TileFieldState.Watered;

        onFullyWatered?.Invoke(this);
    }

    private CropTile GetClosesCropTile(Vector3 seedPosition)
    {
        float minDistance = 5000;
        int closestCropTileIndex = -1;

        for(int i = 0; i < _cropTiles.Count; i++)
        {
            CropTile cropTile = _cropTiles[i];
            float distanceTileSeed = Vector3.Distance(cropTile.transform.position, seedPosition);
        
            if(distanceTileSeed < minDistance)
            {
                minDistance = distanceTileSeed;
                closestCropTileIndex = i;
            }
        }

        if(closestCropTileIndex == -1)
            return null;
        
        return _cropTiles[closestCropTileIndex];
    }

    public void Harvest(Transform harvestSphere)
    {
        float sphereRadius = harvestSphere.localScale.x;

        for(int i = 0; i < _cropTiles.Count; i++)
        {
            if (_cropTiles[i].IsEmpty()) continue;

            float distanceCropTileSphere = Vector3.Distance(harvestSphere.position, _cropTiles[i].transform.position);

            if(distanceCropTileSphere < sphereRadius)
            {
                HarvestTile(_cropTiles[i]);
            }
        }
    }

    private void HarvestTile(CropTile cropTile)
    {
        cropTile.Harvest();

        _tilesHarvested++;

        if(_tilesHarvested == _cropTiles.Count)        
            FieldFullyHarvested();       
    }

    private void FieldFullyHarvested()
    {
        _tilesSown = 0;
        _tilesWatered = 0;
        _tilesHarvested = 0;

        _fieldState = TileFieldState.Empty;

        onFullyHarvested?.Invoke(this);
    }

    [NaughtyAttributes.Button]
    private void InstantlySowTiles()
    {
        for(int i = 0; i < _cropTiles.Count; i++)
            Sow(_cropTiles[i]);
    }
    
    [NaughtyAttributes.Button]
    private void InstantlyWaterTiles()
    {
        for(int i = 0; i < _cropTiles.Count; i++)
            Water(_cropTiles[i]);
    }

    public bool IsEmpty()
    {
        return _fieldState == TileFieldState.Empty;
    }

    public bool IsSown()
    {
        return _fieldState == TileFieldState.Sown;
    }

    public bool IsWatered()
    {
        return _fieldState == TileFieldState.Watered;
    }
}
