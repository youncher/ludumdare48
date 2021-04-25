using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoidMeter : MonoBehaviour
{
    public static float capacity = 100.0f;
    public static float value = 50.0f;
    private float timePassed = 0.0f;
    public static float refreshRate = 1.0f;
    public Slider slider;
    
    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0.0f;
        slider.maxValue = capacity;
        slider.value = value;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= 1/refreshRate)
        {
            value = Mathf.Clamp(value + 1.0f, 0.0f, capacity);
            timePassed = 0.0f;
            SetGloomBar(value);
            // Debug.Log($"meter value is now {value}");
        }
    }
    
    public void SetGloomBar(float gloomValue)
    {
        slider.value = gloomValue;
    }
}
