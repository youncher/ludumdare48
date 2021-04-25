using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class GloomProjectileController : MonoBehaviour, Ld48deeperanddeeper.IGloomProjectileControllerActions
{
    private float moveSpeed = 2.0f;
    private List<GloomProjectile> gloomProjectiles;
    private int activeIdx = -1; // Index of the selected projectile
    private bool spin = false;
    private bool spinClockwise = true;
    
    Ld48deeperanddeeper controls;
    
    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new Ld48deeperanddeeper();
            // Tell the "GloomProjectileController" action map that we want to get told about
            // when actions get triggered.
            controls.GloomProjectileController.SetCallbacks(this);
        }
        controls.Enable();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gloomProjectiles = new List<GloomProjectile>();
        
        // Get gloom projectile inventory
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).CompareTag("GloomProjectile"))
            {
                gloomProjectiles.Add(transform.GetChild(i).gameObject.GetComponent<GloomProjectile>());
            }
        }

        // Select one projectile from inventory
        activeIdx = gloomProjectiles.Count / 2;
    }

    // Update is called once per frame
    void Update()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
// Old input backends are enabled.
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * moveSpeed);
        }
#endif

        if (spin)
        {
            if (spinClockwise)
            {
                transform.Rotate(Vector3.back * moveSpeed);
            }
            else
            {
                transform.Rotate(Vector3.forward * moveSpeed);
            }
        }
        
    }

    public void OnProjectileSpin(InputAction.CallbackContext context)
    {
        spin = true;
        Debug.Log($"{context}");
        Debug.Log($"{context.control}");
        if (context.phase == InputActionPhase.Started)
        {
            spin = true;
            spinClockwise = Keyboard.current.dKey.wasPressedThisFrame ? true : false; // TODO - Check if we want to update this to input not limited to having a keyboard
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            spin = false;
        }
    }
    
    public void ThrowActiveProjectile()
    {
        Debug.Log("Projectiles count: " + gloomProjectiles.Count);
        
        if (gloomProjectiles.Count > 0)
        {
            GloomProjectile activeProjectile = gloomProjectiles[activeIdx];
            
            // Throw projectile
            activeProjectile.ThrowProjectile();

            // Remove from list & destroy projectile
            gloomProjectiles.RemoveAt(activeIdx);
            Debug.Log("Projectile thrown! Projectiles remaining: " + gloomProjectiles.Count);
            
            // Choose a new active projectile
            activeIdx = gloomProjectiles.Count > 0 ? gloomProjectiles.Count / 2 : -1;
        }
        else
        {
            // TODO - Let the user know or make a sound that they don't have any projectiles
            Debug.Log("You have no projectiles to throw!");
        }
    }
}
