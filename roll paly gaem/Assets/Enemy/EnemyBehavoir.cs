using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavoir : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileSpread;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public Vector2 GetDirectionToPlayer(float change = -1f, float randomSpread = -1f)
    {
        Rigidbody2D ccRB = CharacterController.rb;

        Vector2 spread = BulletSpread(randomSpread);
        float distance = Vector3.Distance(transform.position, ccRB.transform.position);
        float relativeSpeed = projectileSpeed - ccRB.velocity.magnitude;

        float time = distance / relativeSpeed;

        Vector2 intercept = (Vector2)ccRB.transform.position + ccRB.velocity * time;
        Vector2 direction = (Vector2.Lerp(intercept, ccRB.transform.position, change) + spread - (Vector2)transform.position).normalized;
        return direction;
    }

    public Vector2 GetDirection(Rigidbody2D rigidbody, float change = -1f, float randomSpread = -1f)
    {
        Vector2 spread = BulletSpread(randomSpread);
        float distance = Vector3.Distance(transform.position, rigidbody.transform.position);
        float relativeSpeed = projectileSpeed - rigidbody.velocity.magnitude;

        float time = distance / relativeSpeed;

        Vector2 intercept = (Vector2)rigidbody.transform.position + rigidbody.velocity * time;
        Vector2 direction = (Vector2.Lerp(intercept, rigidbody.transform.position, change) + spread - (Vector2)transform.position).normalized;
        return direction;
    }

    private Vector2 BulletSpread(float custom = -1f)
    {
        if (custom != -1f) return new Vector2(Random.value * custom, (Random.value * 2 - 0.5f) * custom).normalized;
        return new Vector2(Random.value * projectileSpread, (Random.value * 2 - 0.5f) * projectileSpread).normalized;
    }




}
