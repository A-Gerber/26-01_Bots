using System;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseResources : MonoBehaviour
{
    [SerializeField] private Transform _container;
    
    private List<Resource> _extractedResources;
    private Queue<Resource> _freeResources;
    private List<Resource> _targetResources;

    public event Action<int> ChangedCount;

    public int CountFreeResources => _freeResources.Count;

    private void Awake()
    {
        _extractedResources = new List<Resource>();
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

    public Resource TranslateTargets()
    {
        Resource resource = _freeResources.Dequeue();
        _targetResources.Add(resource);

        return resource;
    }

    public void TakeResource(Resource resource)
    {
        resource.transform.SetParent(_container);
        resource.transform.localPosition = new Vector3();

        _extractedResources.Add(resource);
        _targetResources.Remove(resource);

        ChangedCount?.Invoke(_extractedResources.Count);
    }
}