using UnityEngine;

[RequireComponent(typeof(Mover), typeof(ResoursesRaizer))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Transform _basePosition;

    private Vector3 _targetPoint;
    private bool _canMove = false;

    private Mover _mover;
    private ResoursesRaizer _resoursesRaizer;

    public bool IsBusy { get; private set; } = false;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _resoursesRaizer = GetComponent<ResoursesRaizer>();
    }

    private void OnEnable()
    {
        _resoursesRaizer.PickUped += ReturnToBase;
        _resoursesRaizer.Stoped += StopMove;
        _resoursesRaizer.Puted += SetStatusFree;
    }

    private void OnDisable()
    {
        _resoursesRaizer.PickUped -= ReturnToBase;
        _resoursesRaizer.Stoped -= StopMove;
        _resoursesRaizer.Puted -= SetStatusFree;
    }

    private void Update()
    {      
        if(_canMove)        
            _mover.MoveToTarget(_targetPoint);   
    }

    public void SetTarget(Resource target)
    {
        _resoursesRaizer.SetTarget(target);
        IsBusy = true;

        _targetPoint = target.transform.position;
        _canMove = true;
    }

    private void ReturnToBase() => _targetPoint = _basePosition.position;

    private void SetStatusFree() => IsBusy = false;

    private void StopMove() => _canMove = false;
}