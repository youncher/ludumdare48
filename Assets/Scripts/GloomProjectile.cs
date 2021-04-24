using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomProjectile : MonoBehaviour
{
    private const float MAX_COLOR_VALUE = 100.0f;
    private const float MIN_COLOR_VALUE = 20.0f;
    private const float BLUE_COLOR_DIFF = 10.0f;
    
    private SpriteRenderer spriteRenderer;
    private Color color;
    private float colorValue = 80.0f;
    private bool increaseColorValue = true;
    private float colorChangeSpeed = 115.0f;    
    private float moveSpeed = 1.0f;
    private Vector2 parentPosition;
    private Vector2 projectilePositionTemp;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Increase or decrease rgb color values of projectile
        colorValue += increaseColorValue ? Time.deltaTime * colorChangeSpeed : -Time.deltaTime * colorChangeSpeed;
        
        if (colorValue >= MAX_COLOR_VALUE)
        {
            increaseColorValue = false;
        } else if (colorValue < MIN_COLOR_VALUE)
        {
            increaseColorValue = true;
        }

        // Update projectile color
        spriteRenderer.color = new Color(colorValue/255.0f, colorValue/255.0f, (colorValue + BLUE_COLOR_DIFF)/255.0f);
    }
}
