using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileZone : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GloomProjectile") || other.gameObject.CompareTag("JoyProjectile")) {
            Debug.Log($"{other.gameObject.tag} exited projectile zone");
            Destroy(other.gameObject);
        }
    }
}
