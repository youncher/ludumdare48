using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JoyProjectile : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;
    public float moveSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = new Vector2(
        //         transform.position.x + Time.deltaTime * moveSpeed * direction.x, 
        //         transform.position.y + Time.deltaTime * moveSpeed * yPositionDiff
        //     );
        transform.position = direction * moveSpeed * Time.deltaTime + new Vector2(transform.position.x, transform.position.y);
    }
    public void SetVector(Vector2 vector)
    {
        direction = vector;
    }
}
