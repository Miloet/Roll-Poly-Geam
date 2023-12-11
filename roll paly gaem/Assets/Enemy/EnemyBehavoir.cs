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
    [System.NonSerialized] public Vector2 direction;
    [System.NonSerialized] public MoveType state;
    float moveTimer;

    public static Transform player;

    public enum MoveType
    {
        Idle,
        Approach,
        Run,
        Walk,
        Attack,
        Knockback,
    }

    [Space(5)]
    public float attackCooldown = 5;
    //[System.NonSerialized] 
    public float attackTime;
    public float baseProjectileSpread = 1;
    [Space(10)]

    public GameObject bullet;
    public GameObject body;
    

    [System.NonSerialized] public Rigidbody2D rb;
    [System.NonSerialized] public Animator an;

    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;

        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();

        attackTime = attackCooldown * Random.Range(0.5f, 1.2f);
        direction = transform.position;

        if(player == null) player = CharacterController.rb.transform;

        state = MoveType.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(time <= 0)
        {
            if(CanAttack() && attackTime <= 0) StartCoroutine(AttackBehavoir());
            MoveBehavoir();
            
            time = 0.2f;
        }

        time -= Time.deltaTime;
        attackTime = Mathf.Max(attackTime - Time.deltaTime, 0);
        Move();

        an.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
    }

    public virtual void Move()
    {
        switch(state)
        {
            case MoveType.Idle:

                if (moveTimer <= 0) SwitchState(MoveType.Idle);
                rb.velocity = direction * walkSpeed/2f;
                moveTimer -= Time.deltaTime;

                break;
            case MoveType.Approach:

                rb.velocity = -(transform.position - player.position).normalized * walkSpeed;
                if (Vector2.Distance(transform.position, player.position) < 5) SwitchState(MoveType.Idle);

                break;
            case MoveType.Run:

                rb.velocity = (transform.position - player.position).normalized * walkSpeed;
                if (Vector2.Distance(transform.position, player.position) > 5) SwitchState(MoveType.Idle);

                break;
            case MoveType.Attack:

                if (moveTimer <= 0) SwitchState(MoveType.Idle);
                rb.velocity = Vector2.zero;
                moveTimer -= Time.deltaTime;

                break;
            case MoveType.Knockback:
                if (moveTimer <= 0) SwitchState(MoveType.Idle);
                rb.velocity = knockback * moveTimer*2f;
                moveTimer -= Time.deltaTime;
                break;
        }

    }

    public void SwitchState(MoveType newState, float t = 0)
    {
        state = newState;

        switch (state)
        {
            case MoveType.Idle:

                direction = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized;
                moveTimer = Random.Range(0.2f, 2);

                break;
            case MoveType.Approach:

                break;
            case MoveType.Run:

                break;
            case MoveType.Attack:

                break;
            case MoveType.Knockback:
                moveTimer = 0.5f;
                break;
        }

        if (t != 0) moveTimer = t;
    }
    public virtual void MoveBehavoir()
    {

        if (state != MoveType.Attack || state != MoveType.Knockback)
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance < 2f) SwitchState(MoveType.Run);
            if (distance > 6f) SwitchState(MoveType.Approach);
        }


    }
    public virtual IEnumerator AttackBehavoir()
    {
        attackTime = 9999;
        rb.velocity = Vector2.zero;

        SwitchState(MoveType.Attack, 3f);

        for (int i = 0; i < 1; i++)
        {
            Shoot(9,0.5f,0.5f,5);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);

        attackTime = attackCooldown * Random.Range(0.5f, 1.2f);
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
        
        Bullet firedBullet = g.GetComponent<Bullet>();
        firedBullet.creator = this;
        firedBullet.SetSpeed(direction * projectileSpeed);

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
        SwitchState(MoveType.Knockback);
        if (HP <= 0) Die();
        else an.SetTrigger("Hit");
    }
    public void Die()
    {
        var g = Instantiate(body);
        g.transform.position = transform.position;
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = knockback * 2;
        Destroy(gameObject);
    }

}
