using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip ouchSound;
    public AudioClip throwSound;
    public AudioClip walkSound;
    private AudioSource playerAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // If throwing
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
        
        // TODO: if selecting inventory
        
        // TODO: if got power up

    }
}
