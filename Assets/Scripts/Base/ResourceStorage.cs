using System;
using UnityEngine;

public class ResourceStorage : MonoBehaviour
{
    public event Action<int> ChangedCount;

    public int CountResources { get; private set; } = 0;

    public void TakeResource()
    {
        CountResources++;
        ChangedCount?.Invoke(CountResources);
    }

    public void SpendResources (int count)
    {
        CountResources -= count;
        ChangedCount?.Invoke(CountResources);
    }
}