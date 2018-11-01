using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float flyingHeight;
    public float liftForce;
    public float maxFlySpeed;
    public float flySpeed;
    public LayerMask layerMask;
    [Range(0,1)]
    public float flyHeightChangeChance;
    public Transform[] flyHeightTargets;
    public int currentFlyHeightIndex;
    public float flyHeightDeadZone;

    Rigidbody2D body;
    bool reverseDirection = false;
    private float transitionTimer;

    float liftTime = 0f;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();        
    }

    private void Update()
    {
        transitionTimer += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        flyingHeight = flyHeightTargets[currentFlyHeightIndex].transform.position.y;
        float heightError = flyingHeight - body.position.y;
        
        if (heightError > flyHeightDeadZone)
        {
            body.AddForce(Vector3.up * liftForce);
        } else if (heightError < flyHeightDeadZone)
        {
            body.AddForce(Vector3.up * liftForce * .5f);
        }

        Fly(flySpeed * Time.deltaTime, maxFlySpeed * Time.deltaTime);

        LookInFlyDirection();

        if (reverseDirection)
        {
            body.velocity = new Vector2(-body.velocity.x, -body.velocity.y);
            flySpeed *= -1;
            reverseDirection = false;
        }
    }

    private void LookInFlyDirection()
    {
        if (body.velocity.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (body.velocity.x < 0)
        {
            transform.localScale = Vector3.one;
        }
    }

    void Fly(float speed, float maxSpeed)
    {
        if (Mathf.Abs(body.velocity.x) < maxSpeed)
        {
            body.AddForce(new Vector2(speed, 0), ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (transitionTimer > 3f &&
            (other.CompareTag("FlyHeightTransitionZoneRight") && body.velocity.x < 0 ||
            other.CompareTag("FlyHeightTransitionZoneLeft") && body.velocity.x > 0))
        {
            if (Random.value >= ( 1 - flyHeightChangeChance))
            {
                currentFlyHeightIndex = GetRandomFlyHeightIndex();
                transitionTimer = 0;
            }            
        }
    }
    
    private int GetRandomFlyHeightIndex()
    {
        if (flyHeightTargets.Length == 0)
        {
            throw new System.Exception("No Fly Height Targets");
        }
        if (flyHeightTargets.Length == 1)
        {
            return 0;
        } 

        int flyHeightIndex = currentFlyHeightIndex;

        if (flyHeightIndex == 0)
        {
            flyHeightIndex++;
        } else if (flyHeightIndex == flyHeightTargets.Length - 1)
        {
            flyHeightIndex--;
        } else
        {
            if (Random.value < 0.5f)
            {
                flyHeightIndex--;
            }
            else
            {
                flyHeightIndex++;
            }
        }
        
        return flyHeightIndex;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
