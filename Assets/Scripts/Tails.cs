using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tails : MonoBehaviour
{
    public GameManager gameManager;
    public float moveSpeed = 5f;
    public Animator anim;
    public AudioSource sound;
    public AudioClip flying;
    public bool playedYet;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.hasTails && transform.position.y >= -2.3f)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            anim.SetBool("flying", true);
            if (!playedYet)
            {
                sound.PlayOneShot(flying, 0.5f);
                playedYet = true;
            }
        }
        else if (transform.position.y <= -2.3f)
        {
            if (playedYet)
            {
                sound.Stop();
                playedYet = false;
            }
            anim.SetBool("flying", false);
            
        }
    }
}
