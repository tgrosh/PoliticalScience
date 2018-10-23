using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLimiter : MonoBehaviour {
    public SpriteRenderer renderTarget;
    public Rigidbody2D body;

    private bool isReady = false;
    
    void FixedUpdate () {
        if (isReady && !renderTarget.isVisible)
        {
            body.position = new Vector2((transform.position.x) * -.99f, transform.position.y);
        }

        if (!isReady && renderTarget.isVisible)
        {
            isReady = true;
        }
    }
}
