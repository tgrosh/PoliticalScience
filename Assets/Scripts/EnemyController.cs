using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float flyingHeight;
    public float liftForce;
    public float maxFlySpeed;
    public float flySpeed;
    public LayerMask layerMask;

    Rigidbody2D body;
    bool reverseDirection = false;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, layerMask);
        if (hit.collider != null)
        {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            float heightError = flyingHeight - distance;
            float force = liftForce * heightError - body.velocity.y;
            body.AddForce(Vector3.up * force);
            Fly(flySpeed * Time.deltaTime, maxFlySpeed * Time.deltaTime);
        }
                
        if (body.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (body.velocity.x < 0)
        {
            transform.localScale = Vector3.one;
        }

        if (reverseDirection)
        {
            body.velocity = new Vector2(-body.velocity.x, -body.velocity.y);
            flySpeed *= -1;
            reverseDirection = false;
        }
    }

    void Fly(float speed, float maxSpeed)
    {
        if (Mathf.Abs(body.velocity.x) < maxSpeed)
        {
            body.AddForce(new Vector2(speed, 0), ForceMode2D.Force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("DeathZone"))
        {
            KillSelf();
        }

        string myCollider = collision.otherCollider.gameObject.name;
        string theirCollider = collision.collider.gameObject.name;

        if (myCollider == "Head" && theirCollider == "Body")
        {
            KillSelf();
        }

        Vector2 collisionPoint = transform.InverseTransformPoint(collision.contacts[0].point.x, collision.contacts[0].point.y,0);
        
        if (theirCollider == "Head" || theirCollider == "Body")
        {
            if (collisionPoint.x < 0)
            {
                reverseDirection = true;                
            }
        }
    }
    
    private void KillSelf()
    {
        Destroy(gameObject);
    }
}
