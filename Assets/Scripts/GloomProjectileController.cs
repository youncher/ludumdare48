using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomProjectileController : MonoBehaviour
{
    private float moveSpeed = 2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
}
