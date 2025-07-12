using System;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(ResoursesRaizer))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Transform _basePosition;

    private Vector3 _targetPoint;

    private Mover _mover;
    private ResoursesRaizer _resoursesRaizer;

    public event Action<Unit> Released;

    public bool IsBusy { get; private set; } = false;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _resoursesRaizer = GetComponent<ResoursesRaizer>();
    }

    private void Start()
    {
        Released?.Invoke(this);
    }

    private void OnEnable()
    {
        _resoursesRaizer.PickUped += ReturnToBase;
        _resoursesRaizer.Puted += SetStatusFree;
    }

    private void OnDisable()
    {
        _resoursesRaizer.PickUped -= ReturnToBase;
        _resoursesRaizer.Puted -= SetStatusFree;
    }

    private void Update()
    {      
        if(IsBusy)        
            _mover.MoveToTarget(_targetPoint);   
    }

    public void SetTarget(Resource target)
    {
        _resoursesRaizer.SetTarget(target);
        _targetPoint = target.transform.position;
        IsBusy = true;
    }

    private void ReturnToBase() => _targetPoint = _basePosition.position;

    private void SetStatusFree()
    {
        IsBusy = false; 
        Released?.Invoke(this);
    }
}