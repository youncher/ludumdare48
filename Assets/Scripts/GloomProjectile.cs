using System;
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
    
    private float moveSpeed = 8.0f;
    private Vector2 parentPosition;
    private float xPositionDiff;
    private float yPositionDiff;
    
    private bool activelyThrown = false; // true when projectile is actively moving from being thrown
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activelyThrown)
        {
            // Make thrown projectile travel/move
            transform.position = new Vector2(
                transform.position.x + Time.deltaTime * moveSpeed * xPositionDiff, 
                transform.position.y + Time.deltaTime * moveSpeed * yPositionDiff
            );
        }
        
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

    public void ThrowProjectile()
    {
        // Determine direction of travel by comparing its angle to parent's angle
        parentPosition = transform.parent.position; 
        Vector2 initialProjectilePosition = transform.position;
        xPositionDiff = initialProjectilePosition.x - parentPosition.x;
        yPositionDiff = initialProjectilePosition.y - parentPosition.y;
        activelyThrown = true;
    }
}
