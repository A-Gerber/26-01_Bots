using UnityEngine;

public class BasesSpawner : Spawner<Base>
{
    [SerializeField] private ResourcesHandler _resourcesHandler;
    [SerializeField] private Vector3 _startPointBase;

    private Unit _unit;
    private bool _isFirstCreate = true;

    private void Start()
    {
        Get();
    }

    public void CreateBase(Unit unit, Vector3 flagPosition)
    {
        _startPointBase = flagPosition;
        _unit = unit;

        Get();
    }

    protected override void OnRelease(Base @base)
    {
        base.OnRelease(@base);
        @base.Reset();

        @base.Released -= Release;
    }

    protected override void OnGet(Base @base)
    {
        base.OnGet(@base);
        @base.Init(_resourcesHandler, this);
        @base.transform.position = _startPointBase;

        if (_isFirstCreate)
        {
            @base.CreateStartUnits();
            _isFirstCreate = false;
        }
        else
        {
            @base.AddUnit(_unit);
        }

        @base.Released += Release;
    }
}