using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourcesDetector), typeof(UnitsSpawner))]
public class Base : MonoBehaviour, IFollowable
{
    [SerializeField] private WarehouseResources _warehouse;
    [SerializeField] private Flag _flag;
    [SerializeField] private float _delaySearch;
    [SerializeField] private int _countResourcesForNewUnit = 3;
    [SerializeField] private int _countResourcesForNewBase = 5;
    [SerializeField] private int _minCountUnitsForNewBase = 2;

    private ResourcesDetector _resourcesDetector;
    private UnitsSpawner _unitsSpawner;

    private List<Unit> _units;
    private Queue<Unit> _freeUnits;

    private WaitForSeconds _wait;
    private Coroutine _coroutine;
    private bool _isPriorityNewBase = false;
    private bool _isBuildedNewBase = false;

    public event Action<List<Resource>> FindedResources;
    public event Action<Base> BecomedFreeUnit;
    public event Action<Base> Released;
    public event Action<Unit> BuildedNewBase;

    private void Awake()
    {
        _resourcesDetector = GetComponent<ResourcesDetector>();
        _unitsSpawner = GetComponent<UnitsSpawner>();

        _wait = new WaitForSeconds(_delaySearch);
        _units = new List<Unit>();
        _freeUnits = new Queue<Unit>();

        foreach (var unit in _units)
        {
            Debug.Log(unit.name, unit);
            _freeUnits.Enqueue(unit);
        }
    }

    private void OnEnable()
    {
        _resourcesDetector.FindedResources += TransferFindedResources;
        _warehouse.CollectedResource += CreateUnit;
        _unitsSpawner.Created += AddUnit;

        _coroutine = StartCoroutine(SearchResourcesOverTime());
    }

    private void OnDisable()
    {
        _resourcesDetector.FindedResources -= TransferFindedResources;
        _unitsSpawner.Created -= AddUnit;

        StopCoroutine(_coroutine);
    }

    public Vector3 GetPosition() =>
        transform.position;

    public void ExtractResource(Resource resource) =>
        _freeUnits.Dequeue().ExtractResource(resource, _warehouse);

    public void CreateStartUnits()
    {
        _unitsSpawner.CreateUnit();
        _unitsSpawner.CreateUnit();
        _unitsSpawner.CreateUnit();
    }

    public void AddUnit(Unit unit)
    {
        unit.SetBase(this);
        unit.BecomedFree += AddFreeUnit;
        unit.BuildedNewBase += BuildNewBase;

        _units.Add(unit);
        _freeUnits.Enqueue(unit);
    }

    public void SetFlag(Vector3 point)
    {
        if (_units.Count >= _minCountUnitsForNewBase)
        {
            _flag.gameObject.SetActive(true);
            _flag.transform.position = point;

            if (_isBuildedNewBase == false)
            {
                _isPriorityNewBase = true;
                SetTask();
            }
        }
    }

    private void SetTask()
    {
        if (_freeUnits.Count > 0)
        {
            if (_isPriorityNewBase && _warehouse.CountResources >= _countResourcesForNewBase && _isBuildedNewBase == false)
            {
                _warehouse.SpendResources(_countResourcesForNewBase);
                _freeUnits.Dequeue().MoveToFlag(_flag);

                _isBuildedNewBase = true;
                _isPriorityNewBase = false;
            }
            else
            {
                for (int i = 0; i < _freeUnits.Count; i++)
                {
                    BecomedFreeUnit?.Invoke(this);
                }
            }
        }
    }

    private void BuildNewBase(Unit unit)
    {
        BuildedNewBase?.Invoke(unit);
        RemoveUnit(unit);
        unit.SetStatusFree();
        _isBuildedNewBase = false;
    }

    private IEnumerator SearchResourcesOverTime()
    {
        while (gameObject.activeSelf)
        {
            yield return _wait;

            if (_freeUnits.Count > 0)
                _resourcesDetector.SearchResources();

            SetTask();
        }
    }

    private void TransferFindedResources(List<Resource> findedResources) =>
        FindedResources?.Invoke(findedResources);

    private void AddFreeUnit(Unit unit) =>
        _freeUnits.Enqueue(unit);

    private void CreateUnit()
    {
        if (_isPriorityNewBase == false && _warehouse.CountResources >= _countResourcesForNewUnit)
        {
            _warehouse.SpendResources(_countResourcesForNewUnit);
            _unitsSpawner.CreateUnit();
        }
    }

    private void RemoveUnit(Unit unit)
    {
        unit.BecomedFree -= AddFreeUnit;
        unit.BuildedNewBase -= BuildNewBase;

        _units.Remove(unit);
    }
}