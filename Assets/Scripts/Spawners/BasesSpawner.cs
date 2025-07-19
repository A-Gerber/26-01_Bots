using System;
using System.Collections.Generic;
using UnityEngine;

public class BasesSpawner : Spawner<Base>
{
    [SerializeField] private Vector3 _startPointBase;

    private Unit _unit;
    private bool _isFirstCreate = true;

    public event Action<Base> GetedBase;
    public event Action<Base> ReleasedBase;

    private void Start()
    {
        Get();
    }

    protected override void OnRelease(Base @base)
    {
        base.OnRelease(@base);
        ReleasedBase?.Invoke(@base);

        @base.Released -= Release;
        @base.BuildedNewBase -= CreateBase;
    }

    protected override void OnGet(Base @base)
    {
        base.OnGet(@base);

        if (_isFirstCreate)
        {
            @base.CreateStartUnits();
            _isFirstCreate = false;
        }
        else
        {
            @base.AddUnit(_unit);
        }

        GetedBase?.Invoke(@base);
        @base.transform.position = _startPointBase;

        @base.Released += Release;
        @base.BuildedNewBase += CreateBase;
    }

    private void CreateBase(Unit unit)
    {
        _startPointBase = unit.GetPositionFlag();
        _unit = unit;

        Get();
    }
}