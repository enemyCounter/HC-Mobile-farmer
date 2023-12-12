using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerAnimator))]
public class PlayerHarvestAbility : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform _harvestSphere;

    private PlayerAnimator _playerAnimator;
    private PlayerToolSelector _playerToolSelector;


    [Header(" Settings ")]
    private CropField _currentCropfield;
    private bool _canHarvest;

    // Start is called before the first frame update
    void Start()
    {
        _playerAnimator = GetComponent<PlayerAnimator>();
        _playerToolSelector = GetComponent<PlayerToolSelector>();

        //WaterParticles.onWaterCollided += WaterCollidedCallback;

        CropField.onFullyHarvested += CropFieldFullyHarvestedCallback;

        _playerToolSelector.onToolSelected += ToolSelectedCallback;
    }

    private void OnDestroy()
    {
        //WaterParticles.onWaterCollided -= WaterCollidedCallback;

        CropField.onFullyHarvested -= CropFieldFullyHarvestedCallback;

        _playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool selectedTool)
    {
        if (!_playerToolSelector.CanHarvest())
            _playerAnimator.StopHarvestAnimation();
    }

    private void WaterCollidedCallback(Vector3[] waterPosition)
    {
        if (_currentCropfield == null)
        {
            return;
        }

        _currentCropfield.WaterCollidedCallback(waterPosition);
    }

    private void CropFieldFullyHarvestedCallback(CropField cropField)
    {
        if (cropField == _currentCropfield)
        {
            _playerAnimator.StopHarvestAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
        {
            _currentCropfield = other.GetComponent<CropField>();
            EnteredCropField(_currentCropfield);
        }
    }

    private void EnteredCropField(CropField cropField)
    {
        if (_playerToolSelector.CanHarvest())
        {
            if (_currentCropfield == null)
                _currentCropfield = cropField;

            _playerAnimator.PlayHarvestAnimation();

            if (_canHarvest)
                _currentCropfield.Harvest(_harvestSphere);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsWatered())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            _playerAnimator.StopHarvestAnimation();
            _currentCropfield = null;
        }
    }

    public void HarvestingStartingCallback()
    {
        _canHarvest = true;
    }
    
    public void HarvestingStopingCallback()
    {
        _canHarvest = false;
    }
}