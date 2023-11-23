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

    float destroyDelay = 0;

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
        tag = self.tag;

        if(autoaim && creator != null) SetSpeed(-creator.GetDirectionToPlayer(rb.velocity.magnitude, .5f) * rb.velocity.magnitude);
        else rb.velocity = self.up * rb.velocity.magnitude;
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
}
