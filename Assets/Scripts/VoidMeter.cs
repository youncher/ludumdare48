using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidMeter : MonoBehaviour
{
    public static float capacity = 55.0f;
    public static float value = 50.0f;
    private float timePassed = 0.0f;
    public static float refreshRate = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= 1/refreshRate)
        {
            value = Mathf.Clamp(value + 1.0f, 0.0f, capacity);
            timePassed = 0.0f;
            Debug.Log($"meter value is now {value}");
        }
    }
}
