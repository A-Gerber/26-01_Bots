using UnityEngine;

public class Mover : MonoBehaviour
{    
    [SerializeField] private float _speed;

    public void MoveToTarget(IFollowable target)
    {
        transform.LookAt(target.GetPosition());
        transform.position = Vector3.MoveTowards(transform.position, target.GetPosition(), _speed * Time.deltaTime);
    }
}