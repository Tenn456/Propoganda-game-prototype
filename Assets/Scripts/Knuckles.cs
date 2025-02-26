using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knuckles : MonoBehaviour
{
    public GameManager gameManager;
    public float moveSpeed = 5f;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.hasKnuckles && transform.position.x <= -1.79f)
        {
            anim.SetBool("walking", true);
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }
}
