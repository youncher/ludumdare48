using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Ld48deeperanddeeper.IPlayerActions
{
    // public GloomProjectileController gloomProjectileController;
    public AudioClip ouchSound;
    public AudioClip throwSound;
    public AudioClip walkSound;
    private AudioSource playerAudio;
    private GloomProjectileController gloomProjectileController;
    private AudioSource chargingAudio;

    Ld48deeperanddeeper controls;

    private bool isCharging;
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
        // TODO: if selecting inventory

        // TODO: if got power up

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        // Debug.Log($"{context}");
        // Debug.Log($"{context.control}");
        // Debug.Log($"{context.action}");
        if (context.phase == InputActionPhase.Performed)
        {
            
            Vector2 input = context.ReadValue<Vector2>();
            var destination = transform.position + new Vector3(input.x, input.y, 0);
            if (Gameboard.Validate((int)destination.x, (int)destination.y)) {
                // unset old position in grid
                Gameboard.Vacate((int)transform.position.x, (int)transform.position.y);
                // hop
                transform.position = destination;
                // set new position in grid
                Gameboard.Occupy((int)transform.position.x, (int)transform.position.y);
            }
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {

    }
    public void OnFire(InputAction.CallbackContext context)
    {
        // Debug.Log($"{context}");
        // Debug.Log($"{context.control}");
        if (context.phase == InputActionPhase.Started)
        {
            gloomProjectileController.ThrowActiveProjectile();
            startCharging();
        } else if(context.phase == InputActionPhase.Canceled)
        {
            stopCharging();
        }
    }

    public void OnSelectInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            gloomProjectileController.SelectNextProjectile();
        }
    }

    private void startCharging()
    {
        if (!isCharging)
        {
            chargingAudio.PlayDelayed(0.0f);
            chargingAudio.loop = true;
            isCharging = true;
        }
    }

    private void continueCharging()
    {

    }

    private void stopCharging()
    {
        chargingAudio.loop = false;
        chargingAudio.Stop();
        isCharging = false;
    }
}
