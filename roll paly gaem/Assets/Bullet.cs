using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float force;
    public bool piercing;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(tag != collision.tag)
        {
            switch(collision.tag)
            {
                case "Enemy":
                    collision.GetComponent<EnemyBehavoir>().TakeDamage(damage, rb.velocity.normalized * force);
                    if(!piercing) Destroy(gameObject, 0.1f);

                    break; 
                case "Player":
                    collision.GetComponent<Player>().TakeDamage(damage);
                    if (!piercing) Destroy(gameObject, 0.1f);
                    break; 
                case "Wall":
                    Destroy(gameObject, 0.1f);
                    break; 
            }
        }
    }


}
