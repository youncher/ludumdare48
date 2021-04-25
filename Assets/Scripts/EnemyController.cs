using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    RandomMovementController moveController;
    AudioSource enemyAudio;

    void OnEnable()
    {
        moveController = GetComponent<RandomEnemyMovementController>();
        enemyAudio = GetComponent<AudioSource>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        var target = moveController.Target();
        var gloomProjectile = col.gameObject.GetComponent<GloomProjectile>();
        Debug.Log("Enemy OnCollisionEnter2D");
        if(col.gameObject.tag == "GloomProjectile" && gloomProjectile.GetActivelyThrown()) {
            Debug.Log("collided with GloomProjectile");
            Destroy(col.gameObject);
            // TODO: pitch comparison
            var colliderAudio = col.gameObject.GetComponent<AudioSource>();

            Debug.Log($"collider pitch {colliderAudio.pitch} enemy pitch {enemyAudio.pitch}");
            if (colliderAudio.pitch <= enemyAudio.pitch) {
                if(moveController.IsMoving()) {
                    Gameboard.Vacate((int)target.x, (int)target.y);
                } else {
                    Gameboard.Vacate((int)transform.position.x, (int)transform.position.y);
                }
                Destroy(this.gameObject);
            } else {
                Debug.Log("collided with a thrown gloom projectile who's pitch was higher");
            }
        }
    }
}
