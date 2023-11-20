using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int dmg;
    public int knockback;
    public bool straightKnockback;
    public bool deflect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CompareTag(collision.tag))
        {
            switch (collision.tag)
            {
                case "Enemy":
                    EnemyBehavoir eb = collision.GetComponent<EnemyBehavoir>();
                    if(eb != null) eb.TakeDamage(dmg, GetKnockback(collision.transform));
                    else
                    {
                        if (deflect)
                        {
                            Bullet b = collision.GetComponent<Bullet>();
                            if (b != null)
                            {
                                b.Deflect(transform, true);
                            }
                        }
                    }


                    break;
                case "Player":

                    Player player = collision.GetComponent<Player>();
                    if (player != null) player.TakeDamage(dmg);
                    
                    break;
            }
        }
    }


    public Vector3 GetKnockback(Transform hit)
    {
        if (straightKnockback) return transform.rotation * Vector2.up * knockback;
        else return (hit.position - CharacterController.rb.transform.position).normalized * knockback;
    }
}
