using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLimiter : MonoBehaviour {
    public SpriteRenderer renderTarget;
    public Rigidbody2D body;

	void FixedUpdate () {
        if (!renderTarget.isVisible)
        {
            body.MovePosition(new Vector2((transform.position.x) * -.99f, transform.position.y));
        }
    }
}
