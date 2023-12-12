using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerWaterAbility : MonoBehaviour
{
    [Header(" Elements ")]
    private PlayerAnimator _playerAnimator;
    private PlayerToolSelector _playerToolSelector;

    [Header(" Settings ")]
    private CropField _currentCropfield;

    // Start is called before the first frame update
    void Start()
    {
        _playerAnimator = GetComponent<PlayerAnimator>();
        _playerToolSelector = GetComponent<PlayerToolSelector>();

        WaterParticles.onWaterCollided += WaterCollidedCallback;

        CropField.onFullyWatered += CropFieldFullyWateredCallback;

        _playerToolSelector.onToolSelected += ToolSelectedCallback;
    }

    private void OnDestroy()
    {
        WaterParticles.onWaterCollided -= WaterCollidedCallback;

        CropField.onFullyWatered -= CropFieldFullyWateredCallback;

        _playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool selectedTool)
    {
        if (!_playerToolSelector.CanWater())
            _playerAnimator.StopWaterAnimation();
    }

    private void WaterCollidedCallback(Vector3[] waterPosition)
    {
        if (_currentCropfield == null)
        {
            return;
        }

        _currentCropfield.WaterCollidedCallback(waterPosition);
    }

    private void CropFieldFullyWateredCallback(CropField cropField)
    {
        if (cropField == _currentCropfield)
        {
            _playerAnimator.StopWaterAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsSown())
        {
            _currentCropfield = other.GetComponent<CropField>();
            EnteredCropField(_currentCropfield);
        }

    }

    private void EnteredCropField(CropField cropField)
    {
        if (_playerToolSelector.CanWater())
        {
            if (_currentCropfield == null)
                _currentCropfield = cropField;

            _playerAnimator.PlayWaterAnimation();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsSown())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            _playerAnimator.StopWaterAnimation();
            _currentCropfield = null;
        }
    }
}