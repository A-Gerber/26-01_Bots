using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    protected ObjectPool<T> _pool;

    [SerializeField] private T _prefab;
    [SerializeField] private Transform _container;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    protected virtual void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: () => Create(),
            actionOnGet: (@object) => OnGet(@object),
            actionOnRelease: (@object) => OnRelease(@object),
            actionOnDestroy: (@object) => Destroy(@object.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    protected virtual T Create()
    {
        return Instantiate(_prefab);
    }

    protected virtual void OnRelease(T @object)
    {
        @object.gameObject.SetActive(false);
        @object.transform.SetParent(null);
    }

    protected virtual void OnGet(T @object)
    {
        @object.gameObject.SetActive(true);
        @object.transform.SetParent(_container);
    }

    protected virtual void Get()
    {
        _pool.Get();
    }

    protected virtual void Release(T @object)
    {
        _pool.Release(@object);
    }
}