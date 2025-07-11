using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceWarehouse _warehouse;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _delay;

    private ResourcesDetector _resourcesDetector;
    private TargetsHandler _targetsHandler;

    private WaitForSeconds _wait;
    private Coroutine _coroutine;
    private int _countFreeUnit;

    private void Awake()
    {
        _resourcesDetector = new ResourcesDetector();
        _targetsHandler = new TargetsHandler();

        _wait = new WaitForSeconds(_delay);
        _countFreeUnit = _units.Count;
    }

    private void OnEnable()
    {
        _warehouse.ReceivedResource += DeleteTask;

        _coroutine = StartCoroutine(SearchResourcesOverTime());
    }

    private void OnDisable()
    {
        _warehouse.ReceivedResource -= DeleteTask;

        StopCoroutine(_coroutine);
    }

    private void DeleteTask(Resource resource)
    {
        _targetsHandler.DeletePerformedTasks(resource);
        _countFreeUnit++;
    }

    private IEnumerator SearchResourcesOverTime()
    {
        List<Resource> targets;

        while (gameObject.activeSelf)
        {
            yield return _wait;

            if (_countFreeUnit > 0 && _resourcesDetector.TryGetResourcesInRadius(out targets, transform.position, _searchRadius))
            {
                _targetsHandler.TransferTargets(targets);

                foreach (var unit in _units)
                {
                    if (unit.IsBusy == false && _targetsHandler.Count > 0)
                    {
                        unit.SetTarget(_targetsHandler.SetTarget());
                        _countFreeUnit--;
                    }
                }
            }
        }
    }
}