using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerSowAbility : MonoBehaviour
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

        SeedParticles.onSeedCollided += SeedsCollidedCallback;

        CropField.onFullySown += CropFieldFullySownCallback;

        _playerToolSelector.onToolSelected += ToolSelectedCallback;
    }

    private void OnDestroy()
    {
        SeedParticles.onSeedCollided -= SeedsCollidedCallback;

        CropField.onFullySown -= CropFieldFullySownCallback;

        _playerToolSelector.onToolSelected -= ToolSelectedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToolSelectedCallback(PlayerToolSelector.Tool selectedTool)
    {
        if (!_playerToolSelector.CanSow())
            _playerAnimator.StopSowAnimation();

       
    }

    private void SeedsCollidedCallback(Vector3[] seedsPositions)
    {
        if (_currentCropfield == null)
        {
            return;
        }

        _currentCropfield.SeedCollidedCallback(seedsPositions);
    }

    private void CropFieldFullySownCallback(CropField cropField)
    {
        if(cropField == _currentCropfield)
        {
            _playerAnimator.StopSowAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
        {
            _currentCropfield = other.GetComponent<CropField>();
            EnteredCropField(_currentCropfield);
        }
            
    }

    private void EnteredCropField(CropField cropField)
    {
        if (_playerToolSelector.CanSow())
            _playerAnimator.PlaySowAnimation();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().IsEmpty())
            EnteredCropField(other.GetComponent<CropField>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            _playerAnimator.StopSowAnimation();
            _currentCropfield = null;
        }
    }
}
