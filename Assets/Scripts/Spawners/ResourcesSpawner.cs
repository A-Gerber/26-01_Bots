using System.Collections;
using UnityEngine;

public class ResourcesSpawner : Spawner <Resource>
{
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _minPositionX = -5;
    [SerializeField] private int _maxPositionX = 20;
    [SerializeField] private int _minPositionZ = -15;
    [SerializeField] private int _maxPositionZ = 5;

    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private int _resourcesLayer = 6;    
    private float _positionY = 0.25f;

    protected override void Awake()
    {
        base.Awake();

        _wait = new WaitForSeconds(_repeatRate);
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(GetCubesOverTime());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    protected override void OnRelease(Resource resource)
    {
        base.OnRelease(resource);
        resource.ResetSettings();       

        resource.Released -= Release;
    }

    protected override void OnGet(Resource resource)
    {
        base.OnGet(resource);
        resource.gameObject.layer = _resourcesLayer;

        resource.transform.position = new Vector3(
            UnityEngine.Random.Range(_minPositionX, _maxPositionX + 1),
            _positionY,
            UnityEngine.Random.Range(_minPositionZ, _maxPositionZ + 1));

        resource.Released += Release;
    }

    private IEnumerator GetCubesOverTime()
    {
        while (gameObject.activeSelf)
        {
            yield return _wait;
            Get();
        }
    }
}