using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    RandomMovementController moveController;
    AudioSource enemyAudio;

    public float AnnounceFreqSec = 2.0f;
    public AudioClip AnnounceSound;
    private float sinceLastAnnounce = 0.0f;
    private float minPitch = GloomProjectile.MinGloomPitch();
    private float maxPitch = GloomProjectile.MaxGloomPitch();

    private float minOffsetFromGloom = 10.0f;

    private GameObject playerGO;
    public GameObject JoyProjectilePrefab;

    void OnEnable()
    {
        moveController = GetComponent<RandomEnemyMovementController>();
        enemyAudio = GetComponent<AudioSource>();
        enemyAudio.pitch = Random.Range(minOffsetFromGloom + minPitch, maxPitch);
    }
    void Start()
    {
        sinceLastAnnounce = Random.Range(-AnnounceFreqSec, AnnounceFreqSec);
        playerGO = GameObject.Find("Player").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        sinceLastAnnounce += Time.deltaTime;
        if (sinceLastAnnounce >= AnnounceFreqSec) {
            enemyAudio.PlayOneShot(AnnounceSound, enemyAudio.volume);
            sinceLastAnnounce = 0.0f;
            // Shoot JoyProjectile
            // instantiate
            GameObject projectileGO = Instantiate(JoyProjectilePrefab, transform);
            projectileGO.transform.parent = null;
            // player direction
            JoyProjectile joyProjectile = projectileGO.GetComponent<JoyProjectile>();
            var projectilePos = new Vector2(projectileGO.transform.position.x, projectileGO.transform.position.y);
            var playerPos = new Vector2(playerGO.transform.position.x, playerGO.transform.position.y);
            var diff = playerPos - projectilePos;
            diff.Normalize();
            joyProjectile.SetVector(diff);
        }
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
                var kc = FindObjectOfType<KillIndicator>();
                kc.IncrementKills();
            } else {
                Debug.Log("collided with a thrown gloom projectile who's pitch was higher");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemySuccess") {
            Debug.Log("collided with a trigger tagged EnemySuccess");
            Destroy(gameObject);
            var hi = FindObjectOfType<HealthIndicator>();
            hi.ReduceLife();
        }
    }
}
