using UnityEngine;

public class LoopCounter : MonoBehaviour
{
    [SerializeField] private int _threshold = 10;
    private int _currentValue;

    public int CurrentValue => _currentValue;
    public int Threshold => _threshold;

    private void Start()
    {
        // 触发初始值
        EventBus.Instance.TriggerLoopCountChanged(_currentValue);
    }

    public void Increment()
    {
        _currentValue++;
        EventBus.Instance.TriggerLoopCountChanged(_currentValue);

        if (_currentValue >= _threshold)
        {
            EventBus.Instance.TriggerBossEvent();
            Reset();
        }
    }

    public void Reset()
    {
        _currentValue = 0;
        EventBus.Instance.TriggerLoopCountChanged(_currentValue);
    }
} 