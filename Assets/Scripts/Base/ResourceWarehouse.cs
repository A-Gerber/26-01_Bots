using System;
using UnityEngine;

public class ResourceWarehouse : MonoBehaviour
{
    private int _countResource = 0;

    public event Action<Resource> ReceivedResource;
    public event Action<int> ChangedCount;

    private void Start()
    {
        ChangedCount?.Invoke(_countResource);
    }

    public void TakeResource(Resource resource)
    {
        _countResource++;
        ReceivedResource?.Invoke(resource);
        ChangedCount?.Invoke(_countResource);
    }
}