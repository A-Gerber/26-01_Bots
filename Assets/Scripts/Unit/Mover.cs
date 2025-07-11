using UnityEngine;

public class Mover : MonoBehaviour
{    
    [SerializeField] private float _speed;

    public void MoveToTarget(Vector3 targetPoint)
    {
        transform.LookAt(targetPoint);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, _speed * Time.deltaTime);
    }
}