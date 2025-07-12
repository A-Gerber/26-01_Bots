using System;
using UnityEngine;

public class ResoursesRaizer : MonoBehaviour
{
    private Vector3 _positionLoad = new Vector3(0f, 0.75f, 0.5f);
    private bool _isUnloaded = true;
    private Resource _targetResource;

    public event Action PickUped;
    public event Action Puted;

    private void OnTriggerEnter(Collider other)
    {
        if (_isUnloaded && other.TryGetComponent(out Resource resource) && resource == _targetResource)
        {           
            _isUnloaded = false;

            resource.PickUp(transform, _positionLoad);
            PickUped?.Invoke();
        }
        else if (other.TryGetComponent(out WarehouseResources resourceWarehouse))
        {
            Puted?.Invoke();
            _targetResource.Put();
            resourceWarehouse.TakeResource(_targetResource);

            _targetResource = null;
            _isUnloaded = true;
        }
    }

    public void SetTarget(Resource target)
    {      
        _targetResource = target;
    }
}