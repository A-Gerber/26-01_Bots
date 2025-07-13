using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Released;

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