using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> ReleasedResource;

    public bool IsFree { get; private set; } = true;

    public void PickUp(Transform parent, Vector3 positionLoad)
    {
        transform.SetParent(parent);
        transform.localPosition = positionLoad;
    }

    public void Put()
    {
        transform.SetParent(null);
        IsFree = false;
    }

    public void Use() => ReleasedResource?.Invoke(this);

    public void ResetSettings()
    {
        transform.SetParent(null);
        IsFree = true;
    }
}