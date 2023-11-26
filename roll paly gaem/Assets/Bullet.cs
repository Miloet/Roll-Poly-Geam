using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float force;
    public bool piercing;
    public Rigidbody2D rb;
    public EnemyBehavoir creator;

    public bool HaveTrail = false;
    private GameObject trail;
    public GameObject trailPrefab;

    float destroyDelay = 0;


    private void Start()
    {
        if (HaveTrail)
        {
            trail = Instantiate(trailPrefab);
            trail.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }

    }

    private void FixedUpdate()
    {
        if (HaveTrail)
        {
            trail.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!CompareTag(collision.tag))
        {
            switch(collision.tag)
            {
                case "Enemy":

                    EnemyBehavoir eb = collision.GetComponent<EnemyBehavoir>();
                    if (eb != null)
                    {
                        eb.TakeDamage(damage, rb.velocity.normalized * force);
                        if (!piercing) Destroy(gameObject, destroyDelay);
                    }

                    break; 
                case "Player":

                    Player player = collision.GetComponent<Player>();
                    if (player != null)
                    {
                        player.TakeDamage(damage);
                        if (!piercing) Destroy(gameObject, destroyDelay);
                    }
                    break; 
                case "Wall":
                    Destroy(gameObject, destroyDelay);
                    break; 
            }
        }
    }

    public void Deflect(Transform self, bool autoaim = false)
    {
        if (self.tag != tag)
        {
            tag = self.tag;

            if (autoaim && creator != null) SetSpeed(-creator.GetDirectionToPlayer(rb.velocity.magnitude, .5f) * rb.velocity.magnitude);
            else rb.velocity = self.up * rb.velocity.magnitude;
        }
    }

    public void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(transform.forward, rb.velocity.normalized);
    }

    public void SetSpeed(Vector2 velocity)
    {
        rb.velocity = velocity;
        Rotate();
    }

    private void OnDestroy()
    {
        if (HaveTrail)
        {
            foreach (TrailRenderer tr in trail.GetComponentsInChildren<TrailRenderer>())
            {
                tr.emitting = false;
            }
            foreach (ParticleSystem ps in trail.GetComponentsInChildren<ParticleSystem>())
            {
                ParticleSystem.EmissionModule emission = ps.emission;
                emission.enabled = false;
            }

            Destroy(trail, 5f);
        }
    }
}
