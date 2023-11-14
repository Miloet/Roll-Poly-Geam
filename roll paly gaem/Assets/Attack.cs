using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int dmg;
    public int knockback;
    public bool straightKnockback;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CompareTag(collision.tag))
        {
            switch (collision.tag)
            {
                case "Enemy":
                    collision.GetComponent<EnemyBehavoir>().TakeDamage(dmg, GetKnockback(collision.transform));

                    break;
                case "Player":
                    collision.GetComponent<Player>().TakeDamage(dmg);
                    
                    break;
            }
        }
    }


    public Vector3 GetKnockback(Transform hit)
    {
        if (straightKnockback) return transform.rotation * Vector2.up * knockback;
        else return (hit.position - transform.position).normalized * knockback;
    }
}
