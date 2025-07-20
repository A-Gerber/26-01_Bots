using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private bool _isLoaded = false;

    public event Action<Resource> CollidedWithResource;
    public event Action<Flag> CollidedWithFlag;

    private void OnTriggerEnter(Collider other)
    {
        if (_isLoaded == false && other.TryGetComponent(out Resource resource))
            CollidedWithResource?.Invoke(resource);
        else if (other.TryGetComponent(out Flag flag))
            CollidedWithFlag?.Invoke(flag);

    }

    public void SetLoaded(bool isLoaded)
    {
        _isLoaded = isLoaded;
    }
}