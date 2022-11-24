using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    void Start()
    {
        rb2d = transform.parent.GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb2d.velocity.x > 0) sprite.flipX = true;
        else if (rb2d.velocity.x < 0) sprite.flipX = false;
    }
}
