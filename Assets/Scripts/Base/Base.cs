using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourcesDetector))]
public class Base : MonoBehaviour
{
    [SerializeField] private WarehouseResources _warehouse;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private float _delay;

    private ResourcesDetector _resourcesDetector;

    private Queue<Unit> _freeUnits;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private void Awake()
    {
        _resourcesDetector = GetComponent<ResourcesDetector>();

        _wait = new WaitForSeconds(_delay);
        _freeUnits = new Queue<Unit>();
    }

    private void OnEnable()
    {
        foreach (var unit in _units)
        {
            unit.Released += AddFreeUnit;
        }

        _resourcesDetector.FindedResources += TransferFindedResources;

        _coroutine = StartCoroutine(SearchResourcesOverTime());
    }

    private void OnDisable()
    {
        foreach (var unit in _units)
        {
            unit.Released -= AddFreeUnit;
        }

        _resourcesDetector.FindedResources -= TransferFindedResources;

        StopCoroutine(_coroutine);
    }

    private void TransferFindedResources(List<Resource> findedResources) => _warehouse.TakeFindedResources(findedResources);

    private void AddFreeUnit(Unit unit) => _freeUnits.Enqueue(unit);

    private IEnumerator SearchResourcesOverTime()
    {
        while (gameObject.activeSelf)
        {
            if (_freeUnits.Count > 0)
            {
                _resourcesDetector.SearchResources();

                while (_warehouse.CountFreeResources > 0 && _freeUnits.Count > 0)
                    _freeUnits.Dequeue().SetTarget(_warehouse.TranslateTargets());
            }

            yield return _wait;
        }
    }
}