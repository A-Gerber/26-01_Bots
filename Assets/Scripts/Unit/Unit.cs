using System;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(CollisionHandler), typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    private Vector3 _positionLoad = new Vector3(0f, 0.75f, 0.5f);
    private Base _base;
    private Resource _targetResource;
    private WarehouseResources _targetWarehouse;
    private Flag _targetFlag;
    private IFollowable _followTarget;


    private Mover _mover;
    private CollisionHandler _collisionHandler;

    public event Action<Unit> BecomedFree;
    public event Action<Unit> Released;
    public event Action<Unit> BuildedNewBase;

    public bool IsBusy { get; private set; } = false;
    
    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _collisionHandler = GetComponent<CollisionHandler>();
    }

    private void OnEnable()
    {
        _collisionHandler.CollidedWithResource += PickUpResource;
        _collisionHandler.CollidedWithWarehouse += PutResource;
        _collisionHandler.CollidedWithFlag += BuildBase;
    }

    private void OnDisable()
    {
        _collisionHandler.CollidedWithResource -= PickUpResource;
        _collisionHandler.CollidedWithWarehouse -= PutResource;
        _collisionHandler.CollidedWithFlag -= BuildBase;
    }

    private void Update()
    {      
        if(IsBusy)        
            _mover.MoveToTarget(_followTarget);
    }

    public Vector3 GetPositionFlag() =>
        _targetFlag.transform.position;

    public void SetStatusFree() =>
        IsBusy = false;

    public void SetBase(Base @base) =>
        _base = @base;

    public void MoveToFlag(Flag targetFlag)
    {
        _targetFlag = targetFlag;     
        _followTarget = targetFlag;
        IsBusy = true;
    }

    public void ExtractResource(Resource target, WarehouseResources warehouseResources)
    {
        _followTarget = target;
        _targetResource = target;
        _targetWarehouse = warehouseResources;
        IsBusy = true;
    }

    private void BuildBase (Flag flag)
    {
        if (flag == _targetFlag)       
            BuildedNewBase?.Invoke(this);                                           
    }

    private void PickUpResource(Resource resource)
    {
        if(resource == _targetResource)
        {
            _collisionHandler.SetLoaded(true);
            resource.PickUp(transform, _positionLoad);

            _followTarget = _base;
        }
    }

    private void PutResource (WarehouseResources warehouse)
    {
        if (warehouse == _targetWarehouse)
        {
            IsBusy = false;
            BecomedFree?.Invoke(this);

            warehouse.TakeResource(_targetResource);

            _targetResource = null;
            _collisionHandler.SetLoaded(false);
        }
    }
}