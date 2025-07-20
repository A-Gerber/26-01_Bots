using System.Collections.Generic;
using UnityEngine;

public class ResourcesDetector : MonoBehaviour
{
    [SerializeField] private float _searchRadius;

    private Collider[] _hits;
    private int _numberLayerResource = 6;
    private int _resourcesLayer;
    private int _count = 100;

    private void Awake()
    {
        _hits = new Collider[_count];

        _resourcesLayer = 1 << _numberLayerResource;
    }

    public bool TryGetResourcesInRadius(out List<Resource> resources)
    {
        bool isGot = false;
        resources = new List<Resource>();
        int count = Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, _hits, _resourcesLayer);

        if (count > 0)
        {
            foreach (Collider hit in _hits)
            {
                if (hit != null && hit.TryGetComponent(out Resource resource) && resource.gameObject.layer == _numberLayerResource)
                {
                    resources.Add(resource);
                    isGot = true;
                }
            }
        }

        return isGot;
    }
}