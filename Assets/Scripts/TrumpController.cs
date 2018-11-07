using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpController : MonoBehaviour {
    public float flapForce;
    public float maxFlySpeed;
    public float flySpeed;
    public float maxWalkSpeed;
    public float walkSpeed;
    public SpawnPoint[] spawnPoints;
    public float respawnTime;
    public Animator animator;
    public ParticleSystem flapParticles;
    public AudioSource flapAudio;

    private Rigidbody2D body;
    private float currentSpeed = 0;
    private bool respawning = true;
    private bool reverseDirection;
    private bool moveOffScreen;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //flying
		if (Input.GetKeyDown(KeyCode.LeftControl) && body.velocity.y < maxFlySpeed)
        {
            animator.SetBool("isFlying", true);
            Flap();
            HandleMovement(flySpeed, maxFlySpeed);
        }

        //walking
        if (IsWalking())
        {
            animator.SetBool("isFlying", false);
            HandleMovement(walkSpeed * Time.deltaTime, maxWalkSpeed * Time.deltaTime);
        }

        LookInFlyDirection();        

        if (respawning)
        {
            body.position = FindSpawnPoint().transform.position;
            body.velocity = Vector2.zero;
            respawning = false;
        }

        if (reverseDirection)
        {
            body.velocity = new Vector2(-body.velocity.x, -body.velocity.y);
            reverseDirection = false;
        }

        if (moveOffScreen)
        {
            body.position = new Vector2(0, 10000f);
            moveOffScreen = false;
        }
    }

    private void LookInFlyDirection()
    {
        if (body.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (body.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
    }

    private void Flap()
    {
        flapAudio.Stop();
        flapAudio.Play();
        flapParticles.Play();
        body.AddForce(new Vector2(0, flapForce), ForceMode2D.Impulse);
    }

    private bool IsWalking()
    {
        ContactPoint2D[] contactPoints = new ContactPoint2D[100];
        body.GetContacts(contactPoints);

        foreach (ContactPoint2D contactPoint in contactPoints)
        {
            if (contactPoint.otherCollider != null && contactPoint.otherCollider.gameObject.CompareTag("Body")) {
                return true && body.velocity.y < .05f;
            }
        }

        return false;
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

        Vector2 collisionPoint = transform.InverseTransformPoint(collision.contacts[0].point.x, collision.contacts[0].point.y, 0);

        if (theirCollider == "Head" || theirCollider == "Body")
        {
            if (collisionPoint.x < 0)
            {
                reverseDirection = true;
            }
        }
    }    

    private SpawnPoint FindSpawnPoint()
    {
        SpawnPoint preferredSpawnPoint = null;
        float furthestSpawnPointEnemyDistance = -1f;

        //loop through all spawnpoints
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            //get the spawnpoint's closest enemy distance
            float spawnPointEnemyDistance = spawnPoint.GetClosestEnemyDistance();

            //if not set yet, or spawn point distance is larger than largest
            if (preferredSpawnPoint == null || spawnPointEnemyDistance > furthestSpawnPointEnemyDistance)
            {
                furthestSpawnPointEnemyDistance = spawnPointEnemyDistance;
                preferredSpawnPoint = spawnPoint;
            }
        }

        return preferredSpawnPoint;
    }
    
    private void KillSelf()
    {
        moveOffScreen = true;
        StartCoroutine(WaitAndRespawn(respawnTime));
    }

    IEnumerator WaitAndRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Respawn();        
    }

    void Respawn()
    {
        respawning = true;
    }

    void HandleMovement(float speed, float maxSpeed)
    {
        if (Input.GetKey(KeyCode.LeftArrow) && body.velocity.x > -maxSpeed)
        {
            body.AddForce(new Vector2(-speed, 0), ForceMode2D.Force);
        }

        if (Input.GetKey(KeyCode.RightArrow) && body.velocity.x < maxSpeed)
        {
            body.AddForce(new Vector2(speed, 0), ForceMode2D.Force);
        }
    }
}
