using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourcesDetector), typeof(UnitsSpawner), typeof(ResourceStorage))]
public class Base : MonoBehaviour, IFollowable
{
    [SerializeField] private BotRetreiver _botRetreiver;
    [SerializeField] private Flag _flag;
    [SerializeField] private float _delaySearch;
    [SerializeField] private int _countResourcesForNewUnit = 3;
    [SerializeField] private int _countResourcesForNewBase = 5;
    [SerializeField] private int _minCountUnitsForNewBase = 2;

    private ResourcesDetector _resourcesDetector;
    private UnitsSpawner _unitsSpawner;
    private ResourceStorage _resourceStorage;
    private ResourcesHandler _resourcesHandler;
    private BasesSpawner _basesSpawner;

    private List<Unit> _units;
    private Queue<Unit> _freeUnits;

    private WaitForSeconds _wait;
    private Coroutine _coroutine;
    private bool _isPriorityNewBase = false;
    private bool _isBuildedNewBase = false;

    public event Action<Base> Released;

    private void Awake()
    {
        _resourcesDetector = GetComponent<ResourcesDetector>();
        _unitsSpawner = GetComponent<UnitsSpawner>();
        _resourceStorage = GetComponent<ResourceStorage>();

        _wait = new WaitForSeconds(_delaySearch);
        _units = new List<Unit>();
        _freeUnits = new Queue<Unit>();
    }

    private void OnEnable()
    {
        _unitsSpawner.Created += AddUnit;
        _botRetreiver.CollidedUnitWithResource += HandleUnitWithResource;

        _coroutine = StartCoroutine(SetTasksOverTime());
    }

    private void OnDisable()
    {
        _unitsSpawner.Created -= AddUnit;
        _botRetreiver.CollidedUnitWithResource -= HandleUnitWithResource;

        StopCoroutine(_coroutine);
    }

    public void Init(ResourcesHandler resourcesHandler, BasesSpawner basesSpawner)
    {
        _resourcesHandler = resourcesHandler;
        _basesSpawner = basesSpawner;
    }

    public void Reset()
    {
        _resourcesHandler = null;
        _basesSpawner = null;
    }

    public Vector3 GetPosition() =>
        transform.position;

    public void CreateStartUnits()
    {
        _unitsSpawner.CreateUnit();
        _unitsSpawner.CreateUnit();
        _unitsSpawner.CreateUnit();
    }

    public void AddUnit(Unit unit)
    {
        unit.SetBase(this);
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
                SetTaskUnits();
            }
        }
    }

    private void HandleUnitWithResource(Unit unit)
    {
        unit.PutResource();
        _resourceStorage.TakeResource();

        _freeUnits.Enqueue(unit);
        SetTaskUnits();

        if (_isPriorityNewBase == false && _resourceStorage.CountResources >= _countResourcesForNewUnit)
            CreateUnit();
    }

    private void CreateUnit()
    {
        _resourceStorage.SpendResources(_countResourcesForNewUnit);
            _unitsSpawner.CreateUnit();
    }

    private void SetTaskUnits()
    {
        if (_freeUnits.Count > 0)
        {
            if (_isPriorityNewBase && _resourceStorage.CountResources >= _countResourcesForNewBase && _isBuildedNewBase == false)
            {
                _resourceStorage.SpendResources(_countResourcesForNewBase);
                _freeUnits.Dequeue().MoveToFlag(_flag);

                _isBuildedNewBase = true;
                _isPriorityNewBase = false;
            }
            else
            {
                Resource resource;

                for (int i = 0; i < _freeUnits.Count; i++)
                {
                    if(_resourcesHandler.TryGetFreeResource(out resource))                    
                        _freeUnits.Dequeue().ExtractResource(resource);
                }
            }
        }
    }

    private void BuildNewBase(Unit unit)
    {
        _basesSpawner.CreateBase(unit, _flag.transform.position);
        RemoveUnit(unit);
        unit.SetStatusFree();

        _isBuildedNewBase = false;
        _flag.gameObject.SetActive(false);
    }

    private IEnumerator SetTasksOverTime()
    {
        List<Resource> resources;

        while (gameObject.activeSelf)
        {
            yield return _wait;

            if (_freeUnits.Count > 0 && _resourcesDetector.TryGetResourcesInRadius(out resources))
                _resourcesHandler.HandleResources(resources);

            SetTaskUnits();
        }
    }

    private void RemoveUnit(Unit unit)
    {
        unit.BuildedNewBase -= BuildNewBase;

        _units.Remove(unit);
    }
}