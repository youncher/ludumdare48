using System;
using UnityEngine;

public class GloomProjectile : MonoBehaviour
{
    private const float MAX_GLOOM_PITCH = 3.0f;
    private const float MIN_GLOOM_PITCH = 0.25f; // It's hard to hear the pitch lower than this

    private const float MAX_COLOR_VALUE = 100.0f;
    private const float MIN_COLOR_VALUE = 20.0f;
    private const float BLUE_COLOR_DIFF = 10.0f;

    private GameObject gloomControllerGO;
    private SpriteRenderer spriteRenderer;
    
    private Color color;
    private float colorValue = 80.0f;
    private bool increaseColorValue = true;
    private float colorChangeSpeed = 115.0f;
    
    private float moveSpeed = 8.0f;
    private Vector2 gloomControllerPosition;
    private float xPositionDiff;
    private float yPositionDiff;
    
    private bool activelyThrown = false; // true when projectile is actively moving from being thrown
    private AudioSource gloomAudio;
    
    public GameObject highlightPrefab;

    private void Awake()
    {
        gloomAudio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gloomControllerGO = GameObject.Find("GloomProjectileController").gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        // Projectile has been thrown
        if (activelyThrown)
        {
            gameObject.transform.parent = null;
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
        // Determine direction of travel by comparing its angle to gloomControllerGO's angle
        gloomControllerPosition = gloomControllerGO.transform.position; 
        Vector2 initialProjectilePosition = transform.position;
        xPositionDiff = initialProjectilePosition.x - gloomControllerPosition.x;
        yPositionDiff = initialProjectilePosition.y - gloomControllerPosition.y;
        activelyThrown = true;
    }

    // Highlight this projectile to indicate that it is currently selected
    public void ActivateHighlight()
    {
        PlayActivePitch();
        GameObject highlightObject = Instantiate(highlightPrefab, transform);
        highlightObject.transform.parent = transform;
    }
    
    public void DeactivateHighlight()
    {
        gloomAudio.Stop();
        Destroy(gameObject.transform.GetChild(0).gameObject);
    }

    public bool GetActivelyThrown() {
        return activelyThrown;
    }

    public void PlayActivePitch()
    {
        gloomAudio.Stop();
        gloomAudio.Play();
    }
    
    // Parameter: meterPercent - % of meter used to create gloom
    public void SetGloomPitch(float meterPercent)
    {
        gloomAudio.pitch = Mathf.Clamp(MAX_GLOOM_PITCH - (MAX_GLOOM_PITCH * meterPercent), MIN_GLOOM_PITCH, MAX_GLOOM_PITCH);;
    }
}
