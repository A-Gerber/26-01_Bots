using System;
using UnityEngine;

public class WarehouseResources : MonoBehaviour
{
    public event Action<int> ChangedCount;
    public event Action CollectedResource;

    public int CountResources { get; private set; } = 0;

    public void TakeResource(Resource resource)
    {
        CountResources++;
        resource.Release();

        CollectedResource?.Invoke();
        ChangedCount?.Invoke(CountResources);
    }

    public void SpendResources (int count)
    {
        CountResources -= count;
        ChangedCount?.Invoke(CountResources);
    }
}