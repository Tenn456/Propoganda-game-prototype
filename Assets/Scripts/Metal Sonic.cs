using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalSonic : MonoBehaviour
{
    public GameManager gameManager;
    public float moveSpeed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.metalMoving && transform.position.x >= -14.05f)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(14.36f, -1.79f);
            gameManager.metalMoving = false;
        }
    }
}
