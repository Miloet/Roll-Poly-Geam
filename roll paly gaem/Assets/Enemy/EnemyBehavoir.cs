using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavoir : MonoBehaviour
{
    

    public int MaxHP = 10;
    [System.NonSerialized] public int HP;
    [System.NonSerialized] public Vector2 knockback;
    [Space(10)]

    public float walkSpeed;
    [System.NonSerialized] public Vector2 walkPoint;
    [System.NonSerialized] public bool finishedMoving;
    [System.NonSerialized] public bool altMovement = false;

    [Space(5)]
    public float attackCooldown = 5;
    //[System.NonSerialized] 
    public float attackTime;
    public float baseProjectileSpread = 1;
    [Space(10)]

    public GameObject bullet;
    public GameObject body;
    

    [System.NonSerialized] public Rigidbody2D rb;

    [SerializeField] private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;

        rb = GetComponent<Rigidbody2D>();

        attackTime = attackCooldown;
        walkPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(time <= 0)
        {
            if(CanAttack() && attackTime <= 0) StartCoroutine(AttackBehavoir());
            
            
            time = 0.1f;
        }

        time -= Time.deltaTime;
        attackTime = Mathf.Max(attackTime - Time.deltaTime, 0);
        Move();
        knockback = knockback.normalized * knockback.magnitude * 0.75f;
    }

    public virtual void Move()
    {
        if (!altMovement && !finishedMoving)
        {
            Vector2 direction = (walkPoint - (Vector2)transform.position).normalized;

            rb.velocity = direction * walkSpeed + knockback;

            if (Vector2.Distance(transform.position, walkPoint) < 0.1f)
            {
                finishedMoving = true;
                StartCoroutine(MoveBehavoir());
            }
            else
                if(Time.deltaTime > Random.value) MoveBehavoir();
        }
        else rb.velocity = knockback;
    }
    public virtual IEnumerator MoveBehavoir()
    {
        if(finishedMoving)
        {
            int range = 5;
            float t = Random.Range(.5f, 2f);
            yield return new WaitForSeconds(t);

            Vector2 playerPos = CharacterController.rb.transform.position;
            float distance = Vector2.Distance(transform.position, playerPos);
            for (int i = 0; i < 5; i++)
            {
                walkPoint = (Vector2)transform.position +
                    new Vector2(Random.Range(-range, range),
                    Random.Range(-range, range));

                if (distance > 5 && Vector2.Distance(walkPoint, playerPos) < distance) break;
                if (distance < 2 && Vector2.Distance(walkPoint, playerPos) > distance) break;
            }
            finishedMoving = false;
        }
    }
    public virtual IEnumerator AttackBehavoir()
    {
        altMovement = true;
        attackTime = 9999;
        rb.velocity = Vector2.zero;

        for (int i = 0; i < 3; i++)
        {
            Shoot(9,0.5f,0.5f,5);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);

        attackTime = attackCooldown * Random.Range(0.5f, 1.2f);
        altMovement = false;
    }
    public virtual bool CanAttack()
    {
        return Vector2.Distance(CharacterController.rb.transform.position, transform.position) < 10f;
    }
    public void Shoot(float projectileSpeed ,float change = -1f, float randomSpread = -1f, float untillDeath = 10f)
    {
        var g = Instantiate(bullet);
        g.transform.position = transform.position;
        Vector2 direction = GetDirectionToPlayer(projectileSpeed, change, randomSpread);
        g.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        g.transform.rotation = Quaternion.LookRotation(transform.forward, direction);
        Destroy(g, untillDeath);
    }

    public Vector2 GetDirectionToPlayer(float speed, float change = 0.5f, float randomSpread = -1f)
    {
        Rigidbody2D ccRB = CharacterController.rb;

        Vector2 spread = BulletSpread(randomSpread);
        float distance = Vector3.Distance(transform.position, ccRB.transform.position);
        float relativeSpeed = speed - ccRB.velocity.magnitude;

        float time = distance / relativeSpeed;

        Vector2 intercept = (Vector2)ccRB.transform.position + ccRB.velocity * time;
        Vector2 direction = (Vector2.Lerp(intercept, ccRB.transform.position, change) + spread - (Vector2)transform.position).normalized;
        return direction;
    }

    public Vector2 GetDirection(Rigidbody2D rigidbody, float speed, float change = -1f, float randomSpread = -1f)
    {
        Vector2 spread = BulletSpread(randomSpread);
        float distance = Vector3.Distance(transform.position, rigidbody.transform.position);
        float relativeSpeed = speed - rigidbody.velocity.magnitude;

        float time = distance / relativeSpeed;

        Vector2 intercept = (Vector2)rigidbody.transform.position + rigidbody.velocity * time;
        Vector2 direction = (Vector2.Lerp(intercept, rigidbody.transform.position, change) + spread - (Vector2)transform.position).normalized;
        return direction;
    }

    private Vector2 BulletSpread(float custom = -1f)
    {
        Vector2 spread;

        if (custom != -1f) spread = new Vector2(Random.Range(-1, 1) * custom, Random.Range(-1, 1) * custom); 
        else spread = new Vector2(Random.Range(-1, 1) * baseProjectileSpread, Random.Range(-1, 1) * baseProjectileSpread);


        return Vector2.ClampMagnitude(spread, 1f);
    }

    



    public void TakeDamage(int dmg, Vector2 force)
    {
        HP = Mathf.Clamp(HP - dmg, 0, MaxHP);
        knockback = force;
        if (HP <= 0) Die();
    }
    public void Die()
    {
        var g = Instantiate(body);
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = knockback * 2;
        Destroy(gameObject);
    }

}
