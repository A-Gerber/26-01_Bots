using System;
using UnityEngine;

public class Resource : MonoBehaviour, IFollowable
{
    public event Action<Resource> Released;

    public Vector3 GetPosition() =>
        transform.position;

    public void PickUp(Transform parent, Vector3 positionLoad)
    {
        transform.SetParent(parent);
        transform.localPosition = positionLoad;
    }

    public void Release() => 
        Released?.Invoke(this);

    public void ResetSettings() =>    
        transform.SetParent(null);   
}