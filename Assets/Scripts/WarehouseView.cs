using TMPro;
using UnityEngine;

public class WarehouseView : MonoBehaviour
{
    [SerializeField] private ResourceStorage _warehouse;
    [SerializeField] private TMP_Text _score;

    private void OnEnable()
    {
        _warehouse.ChangedCount += OnScoreChanged;  
    }

    private void OnDisable()
    {
        _warehouse.ChangedCount -= OnScoreChanged;
    }

    private void OnScoreChanged(int count)
    {
        _score.text = count.ToString();
    }
}