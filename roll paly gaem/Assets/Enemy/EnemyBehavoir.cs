using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavoir : MonoBehaviour
{
    

    public int MaxHP = 10;
    [System.NonSerialized] public int HP;

    [Space(10)]

    public float walkSpeed;
    [System.NonSerialized] public Vector2 walkPoint;
    //[System.NonSerialized] 
    public bool finishedMoving;
    //[System.NonSerialized] 
    public bool altMovement = false;

    [Space(5)]
    public float attackCooldown = 2;
    //[System.NonSerialized] 
    public float attackTime;
    [Space(10)]

    public GameObject bullet;
    public float baseProjectileSpread = 1;

    [System.NonSerialized] public Rigidbody2D rb;

    [SerializeField] private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(time <= 0)
        {
            StartCoroutine(MoveBehavoir());

            if(CanAttack() && attackTime <= 0) StartCoroutine(AttackBehavoir());
            
            
            time = 0.1f;
        }

        time -= Time.deltaTime;
        attackTime = Mathf.Max(attackTime - Time.deltaTime, 0);
        Move();
    }

    public virtual void Move()
    {
        if (!altMovement && !finishedMoving)
        {
            Vector2 direction = (walkPoint - (Vector2)transform.position).normalized;

            rb.velocity = direction * walkSpeed;

            if(Vector2.Distance(transform.position, walkPoint) < 0.1f) finishedMoving = true;
        }
    }
    public virtual IEnumerator MoveBehavoir()
    {
        if(finishedMoving)
        {
            walkPoint = (Vector2)transform.position + BulletSpread(5);
            yield return new WaitForSeconds(5);
            finishedMoving = false;
        }
    }
    public virtual IEnumerator AttackBehavoir()
    {
        altMovement = true;
        walkPoint = transform.position;
        attackTime = attackCooldown;
        rb.velocity = Vector2.zero;

        for (int i = 0; i < 3; i++)
        {
            Shoot(5,.5f,0.5f,5);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);

        altMovement = false;
    }
    public virtual bool CanAttack()
    {
        return Vector2.Distance(CharacterController.rb.transform.position, transform.position) < 10f;
    }
    public void Shoot(float projectileSpeed ,float change = -1f, float randomSpread = -1f, float untillDeath = 10f)
    {
        var g = Instantiate(bullet);
        Vector2 direction = GetDirectionToPlayer(projectileSpeed, change, randomSpread);
        g.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        g.transform.rotation = Quaternion.LookRotation(transform.forward, direction);
        Destroy(g, untillDeath);
    }

    public Vector2 GetDirectionToPlayer(float speed, float change = -1f, float randomSpread = -1f)
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
        if (custom != -1f) return new Vector2((Random.value * 2 - 0.5f) * custom, (Random.value * 2 - 0.5f) * custom).normalized;
        return new Vector2(Random.value * baseProjectileSpread, (Random.value * 2 - 0.5f) * baseProjectileSpread).normalized;
    }

    


}
