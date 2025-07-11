using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> ReleasedResource;

    public bool IsFinded { get; private set; } = false;

    public void PickUp(Transform parent, Vector3 positionLoad)
    {
        transform.SetParent(parent);
        transform.localPosition = positionLoad;
    }

    public void Put()
    {
        transform.SetParent(null);
        ReleasedResource?.Invoke(this);
    }

    public void Reset()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        IsFinded = false;
    }

    public void SetStatusFinded()
    {
        IsFinded = true;
    }
}