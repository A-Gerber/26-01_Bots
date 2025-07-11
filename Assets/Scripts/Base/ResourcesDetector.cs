using System.Collections.Generic;
using UnityEngine;

public class ResourcesDetector
{
    private Collider[] _hits;
    private int _resourcesLayer = 1 << 6;
    private int _count = 10;

    public ResourcesDetector()
    {
        _hits = new Collider[_count];
    }

    public bool TryGetResourcesInRadius(out List<Resource> targets, Vector3 position, float radius)
    {
        bool isGot = false;
        targets = new List<Resource>();

        int count = Physics.OverlapSphereNonAlloc(position, radius, _hits, _resourcesLayer);

        if (count > 0)
        {
            foreach (Collider hit in _hits)
            {
                if (hit != null && hit.TryGetComponent(out Resource resource) && resource.gameObject.activeSelf && resource.IsFinded == false)
                {
                    targets.Add(resource);
                    isGot = true;
                }
            }
        }

        return isGot;
    }
}