using UnityEngine;
using TMPro;

public class LoopCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterText;
    //[SerializeField] private TextMeshProUGUI _thresholdText;
    [SerializeField] private LoopCounter _loopCounter;

    private void Start()
    {
        //_loopCounter = FindObjectOfType<LoopCounter>();
        if (_loopCounter != null)
        {
            UpdateUI(_loopCounter.CurrentValue);
            //UpdateThreshold(_loopCounter.Threshold);
        }
    }

    private void OnEnable()
    {
        EventBus.Instance.OnLoopCountChanged += UpdateUI;
    }

    private void OnDisable()
    {
        EventBus.Instance.OnLoopCountChanged -= UpdateUI;
    }

    private void UpdateUI(int value)
    {
        if (_counterText != null)
        {
            _counterText.text = $"Loop: {value}";
        }
    }

    //private void UpdateThreshold(int threshold)
    //{
        //if (_thresholdText != null)
        //{
            //_thresholdText.text = $"/ {threshold}";
        //}
    //}
} 