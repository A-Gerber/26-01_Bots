using System.Collections;
using UnityEngine;

public class ResourcesSpawner : Spawner <Resource>
{
    [SerializeField] private float _repeatRate;

    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private int _positionY = 0;
    private int _minPositionX = -5;
    private int _maxPositionX = 20;
    private int _minPositionZ = -15;
    private int _maxPositionZ = 5;

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