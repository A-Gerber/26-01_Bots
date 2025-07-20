using UnityEngine;

public class ClickedHandler : MonoBehaviour
{
    private const int MouseButton = 0;

    [SerializeField] private Camera _camera;
    [SerializeField] private Ray _ray;

    private bool _isSelectedBase = false;
    private float _distance = 100f;
    private Base _target;

    private void Update()
    {
        if (Input.GetMouseButtonDown(MouseButton))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(_ray, out hit, _distance))
            {
                if (hit.transform.TryGetComponent(out Base @base))
                {
                    _isSelectedBase = true;
                    _target = @base;
                }
                else if (_isSelectedBase && hit.transform.TryGetComponent(out Ground ground))
                {
                    _target.SetFlag(hit.point);
                    _isSelectedBase = false;
                }
                else
                {
                    _isSelectedBase = false;
                }
            }
        }
    }
}