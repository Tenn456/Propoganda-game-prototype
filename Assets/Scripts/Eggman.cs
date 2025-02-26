using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eggman : MonoBehaviour
{
    public GameManager gameManager;
    public float moveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.hasEggman && transform.position.x >= 5.83f)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
    }
}
