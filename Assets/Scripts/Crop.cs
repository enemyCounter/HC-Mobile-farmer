using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform _croprenderer;
    [SerializeField] private ParticleSystem _harvestParticles;

    private Vector3 _cornGrownSize = new Vector3(1f, 1f, 1f);

    public void ScaleUp()
    {
        _croprenderer.gameObject.LeanScale(_cornGrownSize, 1).setEase(LeanTweenType.easeOutBounce);
    }
    
    public void ScaleDown()
    {
        _croprenderer.gameObject.LeanScale(Vector3.zero, 1).
            setEase(LeanTweenType.easeInBack).setOnComplete(DestroyCrop);

        _harvestParticles.gameObject.SetActive(true);
        _harvestParticles.transform.parent = null;
        _harvestParticles.Play();
    }

    private void DestroyCrop()
    {
        Destroy(gameObject);
    }
}
