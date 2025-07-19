using System;
using UnityEngine;

public class UnitsSpawner : Spawner<Unit>
{
    [SerializeField] private Transform _pointSpawn;

    public event Action<Unit> Created;

    public void CreateUnit() =>
        Get();

    protected override void OnRelease(Unit unit)
    {
        base.OnRelease(unit);

        unit.Released -= Release;
    }


    protected override void OnGet(Unit unit)
    {
        base.OnGet(unit);

        unit.transform.position = _pointSpawn.position;
        Created?.Invoke(unit);

        unit.Released += Release;
    }
}