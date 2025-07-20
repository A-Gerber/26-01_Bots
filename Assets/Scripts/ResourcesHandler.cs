using System.Collections.Generic;
using UnityEngine;

public class ResourcesHandler : MonoBehaviour
{
    private const int DefaultLayer = 0;

    private Queue<Resource> _freeResources;

    private void Awake()
    {
        _freeResources = new Queue<Resource>();
    }

    public void HandleResources(List<Resource> findedResources)
    {
        foreach (var resource in findedResources)
        {
            if (_freeResources.Contains(resource) == false)
                _freeResources.Enqueue(resource);
        }
    }

    public bool TryGetFreeResource(out Resource resource)
    {
        bool isGot = false;
        resource = null;

        if (_freeResources.Count > 0)
        {
            resource = _freeResources.Dequeue();
            resource.gameObject.layer = DefaultLayer;

            isGot = true;
        }

        return isGot;
    }
}