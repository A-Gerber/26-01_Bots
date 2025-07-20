using System;
using UnityEngine;

public class BotRetreiver : MonoBehaviour
{
    public event Action<Unit> CollidedUnitWithResource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Unit unit) && unit.IsLoaded)
            CollidedUnitWithResource?.Invoke(unit);
    }
}
