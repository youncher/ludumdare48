using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoidMeter : MonoBehaviour
{
    public static float capacity = 100.0f;
    public static float value = 50.0f;
    public static float rate = 1.0f;
    public Slider slider;
    
    private float timePassed = 0.0f;

    private bool chargeUp = true;
    
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
        if (chargeUp)
        {
            IncreaseMeter();
        }
    }

    public void IncreaseMeter()
    {
        ChangeMeter(1.0f, rate);
    }
    
    // Returns true if there are still things to drain, false if empty
    public bool DecreaseMeter()
    {
        ChangeMeter(-10.0f, rate * 2.5f);

        if (value != 0)
        {
            return true;
        }
        return false;
    }

    private void ChangeMeter(float x, float refreshRate)
    {
        timePassed += Time.deltaTime;
        if(timePassed >= 1/refreshRate)
        {
            value = Mathf.Clamp(value + (1.0f * x), 0.0f, capacity);
            timePassed = 0.0f;
            // Debug.Log($"meter value is now {value}");
            UpdateGloomBarUI(value);
        }
    }
    
    public void UpdateGloomBarUI(float gloomValue)
    {
        slider.value = gloomValue;
    }

    public void StopAutoCharge()
    {
        chargeUp = false;
    }
    
    public void StartAutoCharge()
    {
        chargeUp = true;
    }

    public float GetValue()
    {
        return value;
    }

    public float GetCapacity()
    {
        return capacity;
    }
}
