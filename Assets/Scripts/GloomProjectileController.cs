using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class GloomProjectileController : MonoBehaviour, Ld48deeperanddeeper.IGloomProjectileControllerActions
{
    public GameObject gloomPrefab;
    public GameObject gloomPosition1;
    public GameObject gloomPosition2;
    public GameObject gloomPosition3;
    
    private const int MAX_GLOOM_PROJECTILES = 3;
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
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject positionObject = transform.GetChild(i).gameObject;
            if (positionObject.transform.GetChild(0).CompareTag("GloomProjectile"))
            {
                gloomProjectiles.Add(positionObject.transform.GetChild(0).gameObject.GetComponent<GloomProjectile>());
            }
        }

        AutoSetActiveProjectile();
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
        // Debug.Log($"{context}");
        // Debug.Log($"{context.control}");
        if (context.phase == InputActionPhase.Started)
        {
            spin = true;
            spinClockwise = context.ReadValue<float>() > 0;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            spin = false;
        }
    }

    public void OnCheckGloomPitch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            gloomProjectiles[activeIdx].PlayActivePitch();
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
            if (gloomProjectiles.Count > 0)
            {
                AutoSetActiveProjectile();
            }
            else
            {
                activeIdx = -1;
            }
        }
        else
        {
            // TODO - Let the user know or make a sound that they don't have any projectiles
            Debug.Log("You have no projectiles to throw!");
        }
    }

    private void AutoSetActiveProjectile()
    {
        activeIdx = gloomProjectiles.Count / 2;
        gloomProjectiles[activeIdx].ActivateHighlight();
    }

    // Cycle through inventory to select projectile
    public void SelectNextProjectile()
    {
        // No need to select projectiles if you have 0-1 of them
        if (gloomProjectiles.Count <= 1)
        {
            return;
        }

        // Remove highlight of current active projectile
        gloomProjectiles[activeIdx].DeactivateHighlight();
        
        // Select next projectile by counting up idx position. Or if at the end, go back to the beginning of inventory
        if (activeIdx + 1 < gloomProjectiles.Count)
        {
            activeIdx++; 
        }
        else
        {
            activeIdx = 0;
        }
        gloomProjectiles[activeIdx].ActivateHighlight();
    }

    public bool CanAddAnotherGloom()
    {
        return gloomProjectiles.Count < MAX_GLOOM_PROJECTILES;
    }

    // TODO - use the parameter (% of juice obtained from meter) to set the pitch of the new gloom projectile
    // Add gloom ands sets position and highlight
    // Parameter: meterPercent - % of meter used to create gloom
    public void AddGloom(float meterPercent)
    {
        if (activeIdx >= 0)
        {
            gloomProjectiles[activeIdx].DeactivateHighlight();
        }
        
        if (gloomProjectiles.Count < MAX_GLOOM_PROJECTILES)
        {
            GameObject gloomObject = Instantiate(gloomPrefab, transform);
            GloomProjectile gloomProjectile = gloomObject.GetComponent<GloomProjectile>();
            gloomProjectile.SetGloomPitch(meterPercent);
            gloomProjectiles.Add(gloomProjectile);
            activeIdx = gloomProjectiles.Count - 1;
            gloomProjectile.ActivateHighlight();
            UpdateGloomPositionAndParent(gloomObject);
        }
    }

    // Set position of new gloom
    private void UpdateGloomPositionAndParent(GameObject gloomObject)
    {
        if (gloomProjectiles.Count == 1)
        {
            gloomObject.transform.position = gloomPosition1.transform.position;
            gloomObject.transform.parent = gloomPosition1.transform;
        } else if (gloomProjectiles.Count == 2)
        {
            gloomObject.transform.position = gloomPosition2.transform.position;
            gloomObject.transform.parent = gloomPosition2.transform;
        } else if (gloomProjectiles.Count == 3)
        {
            gloomObject.transform.position = gloomPosition3.transform.position;
            gloomObject.transform.parent = gloomPosition3.transform;
        }
    }
}
