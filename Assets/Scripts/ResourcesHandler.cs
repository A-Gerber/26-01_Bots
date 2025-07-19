using System.Collections.Generic;
using UnityEngine;

public class ResourcesHandler : MonoBehaviour
{
    private const int DefaultLayer = 0;

    [SerializeField] private BasesSpawner _baseSpawner;

    private List<Base> _bases;
    private Queue<Resource> _freeResources;

    private void Awake()
    {
        _freeResources = new Queue<Resource>();
        _bases = new List<Base>();
    }

    private void OnEnable()
    {
        foreach (var @base in _bases)
        {
            @base.FindedResources += HandleResources;
            @base.BecomedFreeUnit += ExtractResource;
        }

        _baseSpawner.GetedBase += AddBase;
        _baseSpawner.ReleasedBase += RemoveBase;
    }

    private void OnDisable()
    {
        foreach (var @base in _bases)
        {
            @base.FindedResources -= HandleResources;
            @base.BecomedFreeUnit -= ExtractResource;
        }

        _baseSpawner.GetedBase -= AddBase;
        _baseSpawner.ReleasedBase -= RemoveBase;
    }

    private void AddBase(Base @base)
    {
        _bases.Add(@base);

        @base.FindedResources += HandleResources;
        @base.BecomedFreeUnit += ExtractResource;
    }

    private void RemoveBase(Base @base)
    {
        _bases.Remove(@base);

        @base.FindedResources -= HandleResources;
        @base.BecomedFreeUnit -= ExtractResource;
    }

    private void ExtractResource(Base @base)
    {
        if (_freeResources.Count > 0)
        {
            Resource resource = _freeResources.Dequeue();
            resource.gameObject.layer = DefaultLayer;

            @base.ExtractResource(resource);
        }
    }

    private void HandleResources(List<Resource> findedResources)
    {
        foreach (var resource in findedResources)
        {
            if (_freeResources.Contains(resource) == false)
                _freeResources.Enqueue(resource);
        }
    }
}