using System;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseResources : MonoBehaviour
{
    private Queue<Resource> _freeResources;
    private List<Resource> _targetResources;
    private int _countResources = 0;

    public event Action<int> ChangedCount;

    public int CountFreeResources => _freeResources.Count;

    private void Awake()
    {
        _freeResources = new Queue<Resource>();
        _targetResources = new List<Resource>();
    }

    public void TakeFindedResources(List<Resource> findedResources)
    {
        foreach (var resource in findedResources)
        {
            if (_freeResources.Contains(resource) == false && _targetResources.Contains(resource) == false)
                _freeResources.Enqueue(resource);
        }
    }

    public Resource GetTarget()
    {
        Resource resource = _freeResources.Dequeue();
        _targetResources.Add(resource);

        return resource;
    }

    public void TakeResource(Resource resource)
    {
        _countResources++;
        resource.Release();
        _targetResources.Remove(resource);

        ChangedCount?.Invoke(_countResources);
    }
}