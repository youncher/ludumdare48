using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Ld48deeperanddeeper.IPlayerActions
{
    public VoidMeter voidMeter;
    private AudioSource playerAudio;
    private GloomProjectileController gloomProjectileController;
    private AudioSource chargingAudio;
    
    Ld48deeperanddeeper controls;

    private bool isCharging;
    private float chargeBegValue = 0.0f; // Value of meter when we started charging
    private float chargeEndValue = 0.0f; // Value of meter when we stopped charging
    public bool gameOver { get; set; }
    
    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new Ld48deeperanddeeper();
            // Tell the "Player" action map that we want to get told about
            // when actions get triggered.
            controls.Player.SetCallbacks(this);
        }
        controls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        gloomProjectileController = transform.GetChild(0).GetComponent<GloomProjectileController>();
        var audioSources = GetComponents<AudioSource>();
        playerAudio = audioSources[0];
        chargingAudio = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
// Old input backends are enabled.
        // If throwing gloom projectile
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gloomProjectileController.ThrowActiveProjectile();
            playerAudio.PlayOneShot(throwSound, 1.0f);
        }

        // TODO: if got hit

        // When player is walking
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position = new Vector2(transform.position.x - 1, transform.position.y);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position = new Vector2(transform.position.x + 1, transform.position.y);

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        }
#endif
        if (isCharging)
        {
            bool meterHasMoreJuice = voidMeter.DecreaseMeter();
            float maxPitch = GloomProjectile.MaxGloomPitch();
            float minPitch = GloomProjectile.MinGloomPitch();

            float currentCharge = voidMeter.GetValue();
            float meterMaxValue = voidMeter.GetCapacity();
            float meterJuiceObtained = chargeBegValue - currentCharge;
            float meterJuiceObtainedPercentage = meterJuiceObtained / meterMaxValue;

            chargingAudio.pitch = Mathf.Clamp(maxPitch - (maxPitch * meterJuiceObtainedPercentage), minPitch, maxPitch);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (gameOver) return;
        
        // Debug.Log($"{context}");
        // Debug.Log($"{context.control}");
        // Debug.Log($"{context.action}");
        if (context.phase == InputActionPhase.Performed)
        {
            
            Vector2 input = context.ReadValue<Vector2>() * Gameboard.worldToGridRatio;
            var destination = transform.position + new Vector3(input.x, input.y, 0);
            if (Gameboard.ValidateByWorldPos(destination)) {
                // unset old position in grid
                Gameboard.VacateByWorldPos(new Vector2(transform.position.x, transform.position.y));
                // hop
                transform.position = destination;
                // set new position in grid
                Gameboard.OccupyByWorldPos(new Vector2(transform.position.x, transform.position.y));
            }
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (gameOver) return;
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        if (gameOver) return;
        
        // Debug.Log($"{context}");
        // Debug.Log($"{context.control}");
        if (context.phase == InputActionPhase.Started)
        {
            gloomProjectileController.ThrowActiveProjectile();
        }
    }

    public void OnSelectInventory(InputAction.CallbackContext context)
    {
        if (gameOver) return;
        
        if (context.phase == InputActionPhase.Started)
        {
            gloomProjectileController.SelectNextProjectile();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (gameOver) return;
        
        if (context.phase == InputActionPhase.Started)
        {
            StartCharging();
        } else if(context.phase == InputActionPhase.Canceled)
        {
            StopCharging();
        }
    }

    // Plays charging sound, stops voidMeter autocharge while charging
    private void StartCharging()
    {
        if (!isCharging && gloomProjectileController.CanAddAnotherGloom())
        {
            chargingAudio.PlayDelayed(0.0f);
            chargingAudio.loop = true;
            isCharging = true;
            voidMeter.StopAutoCharge();
            chargeBegValue = voidMeter.GetValue();
        }
    }

    // Stops charging sound; gets amount charged; creates new gloom from the juice obtained
    private void StopCharging()
    {
        chargingAudio.loop = false;
        chargingAudio.Stop();
        isCharging = false;
        chargeEndValue = voidMeter.GetValue();
        voidMeter.StartAutoCharge();
        float meterMaxValue = voidMeter.GetCapacity();
        float meterJuiceObtained = chargeBegValue - chargeEndValue;
        float meterJuiceObtainedPercentage = meterJuiceObtained / meterMaxValue;

        // If was able to get gloom juice from meter, then create gloom from it
        if (meterJuiceObtained > 0.0f)
        {
            gloomProjectileController.AddGloom(meterJuiceObtainedPercentage);
        }

        chargeBegValue = 0.0f;
        chargeEndValue = 0.0f;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        CollisionHandler(col);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        CollisionHandler(col);
    }
    private void CollisionHandler(Collision2D col)
    {
        if (!col.gameObject.CompareTag("JoyProjectile"))
        {
            return;
        }
        Debug.Log("Player OnCollision 2D JoyProjectile");
        Destroy(col.gameObject);            
        var hi = FindObjectOfType<HealthIndicator>();
        hi.ReduceLife();
    }
}
