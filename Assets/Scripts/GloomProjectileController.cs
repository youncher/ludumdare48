using System.Collections.Generic;
using UnityEngine;

public class GloomProjectileController : MonoBehaviour
{
    private float moveSpeed = 2.0f;
    private List<GloomProjectile> gloomProjectiles;
    private int activeIdx = -1; // Index of the selected projectile
    
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
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * moveSpeed);
        }
    }

    public void ThrowActiveProjectile()
    {
        Debug.Log("Projectiles:" + gloomProjectiles.Count);
        Debug.Log("Active Projectile Idx:" + activeIdx);
        
        if (gloomProjectiles.Count > 0)
        {
            GloomProjectile activeProjectile = gloomProjectiles[activeIdx];
            
            // TODO: throw projectile
            Debug.Log("Throwing projectile:" + activeIdx);
            
            // remove from list & destroy projectile
            gloomProjectiles.RemoveAt(activeIdx);
            Destroy(activeProjectile);
            
            // choose a new active projectile
            activeIdx = gloomProjectiles.Count > 0 ? gloomProjectiles.Count / 2 : -1;
            Debug.Log("New activeIdx is now:" + activeIdx);
        }
        else
        {
            // TODO - Let the user know or make a sound that they don't have any projectiles
            Debug.Log("You have no projectiles to throw!");
        }
    }
}
