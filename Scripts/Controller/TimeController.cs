using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    
    [SerializeField] private Button slower;
    [SerializeField] private Button faster;
    [SerializeField] private TextMeshProUGUI timeScaleText;

    private bool isTimeHalf = false;
    private bool isTimeDoubled = false;
   
    private void Start()
    {
        faster.onClick.AddListener(TimeAcceleration);
        slower.onClick.AddListener(TimeDeceleration);
    }

    private void UpdateTimeScale()
    {
        timeScaleText.text = "현재 적용 속도 : " + Time.timeScale.ToString("F1");
    }
    private void TimeDeceleration()
    {
        if (isTimeDoubled)
        {
            Time.timeScale = 1f; 
            isTimeDoubled = false;
        }

        else
        {
            Time.timeScale = 0.5f; 
            isTimeHalf = true;
        }

        UpdateTimeScale();
    }

    private void TimeAcceleration() 
    {
        if (isTimeHalf)
        {
            Time.timeScale = 1f; 
            isTimeHalf = false;
        }

        else
        {
            Time.timeScale = 3f; 
            isTimeDoubled = true;
        }

        UpdateTimeScale();
    }


}
