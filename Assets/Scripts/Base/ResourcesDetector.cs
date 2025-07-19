using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDetector : MonoBehaviour
{
    [SerializeField] private float _searchRadius;

    private Collider[] _hits;
    private List<Resource> _targets;
    private int _numberLayerResource = 6;
    private int _resourcesLayer;
    private int _count = 100;

    public event Action<List<Resource>> FindedResources;

    private void Awake()
    {
        _hits = new Collider[_count];
        _targets = new List<Resource>();

        _resourcesLayer = 1 << _numberLayerResource;
    }

    public void SearchResources()
    {
        if(TryGetResourcesInRadius())
        {
            FindedResources?.Invoke(_targets);
            _targets.Clear();
        }
    }

    private bool TryGetResourcesInRadius()
    {
        bool isGot = false;
        int count = Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, _hits, _resourcesLayer);

        if (count > 0)
        {
            foreach (Collider hit in _hits)
            {
                if (hit != null && hit.TryGetComponent(out Resource resource) && resource.gameObject.layer == _numberLayerResource)
                {
                    _targets.Add(resource);
                    isGot = true;
                }
            }
        }

        return isGot;
    }
}