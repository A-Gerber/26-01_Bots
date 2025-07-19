using UnityEngine;

public class Flag : MonoBehaviour, IFollowable
{
    public Vector3 GetPosition() =>
        transform.position;
}