using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float walkSpeed = 1f;
    public float originalWalkSpeed;
    public static Vector2 movementInput;

    //Componants

    public static Rigidbody2D rb;
    Animator an;

    private Camera cam;

    Buffer attack;

    bool altAttack;
    float attackCooldown;

    GameObject attackAim;

    GameObject whip;
    Animator whipAnimator;
    SpriteRenderer whipSR;
    GameObject sword;
    Animator swordAnimator;
    SpriteRenderer swordSR;

    Color ready = new Color(1, 1, 1, 1);
    Color notReady = new Color(0.8f, 0.8f, 0.8f, 1);

    Buffer shoot;
    bool altFire;
    float shootCooldown;

    GameObject shootAim;

    Transform gunBarrel;
    Animator gunAnimator;
    SpriteRenderer gunSR;
    ParticleSystem gunFlash;

    public float gunProjectileSpeed;
    public GameObject gunBullet;

    
    Transform sniperBarrel;
    Animator sniperAnimator;
    SpriteRenderer sniperSR;
    ParticleSystem sniperFlash;

    public float sniperProjectileSpeed;
    public GameObject sniperBullet;

    // Start is called before the first frame update
    void Start()
    {
        originalWalkSpeed = walkSpeed;
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();

        attackAim = transform.Find("Aim").gameObject;

        sword = attackAim.transform.Find("Sword").gameObject;
        swordAnimator = sword.GetComponent<Animator>();
        swordSR = sword.GetComponent<SpriteRenderer>();

        whip = attackAim.transform.Find("Whip").gameObject;
        whipAnimator = whip.GetComponent<Animator>();
        whipSR = whip.GetComponent<SpriteRenderer>();

        whipSR.color = notReady;
        swordSR.color = ready;

        attack = Buffer.SetBuffer(gameObject, 0.15f);



        shootAim = transform.Find("GunAim").gameObject;

        var sniper = shootAim.transform.Find("Sniper").gameObject;
        //sniperAnimator = sniper.GetComponent<Animator>();
        sniperSR = sniper.GetComponent<SpriteRenderer>();
        sniperBarrel = sniper.transform.Find("Barrel");
        sniperFlash = sniperBarrel.GetComponent<ParticleSystem>();


        var gun = shootAim.transform.Find("Gun").gameObject;
        //gunAnimator = gun.GetComponent<Animator>();
        gunSR = gun.GetComponent<SpriteRenderer>();
        gunBarrel = gun.transform.Find("Barrel");
        gunFlash = gunBarrel.GetComponent<ParticleSystem>();

        gunSR.color = ready;
        sniperSR.color = notReady;

        shoot = Buffer.SetBuffer(gameObject, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Attack") != 0) attack.Pressed();

        #region//Movement

        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (movementInput.x != 0 || movementInput.y != 0)
        {
            an.SetFloat("Horizontal", movementInput.x);
            an.SetFloat("Vertical", movementInput.y);
        }
        rb.velocity = Vector2.ClampMagnitude(movementInput, 1f) * walkSpeed;

        #endregion


        #region Attack

        if (attackCooldown <= 0)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)transform.position - mousePos;
            attackAim.transform.rotation = Quaternion.LookRotation(transform.forward, -direction);
        }

        if (attackCooldown <= 0 && attack.GetPress())
        {
            attack.Unpress();

            if (!altAttack) StartCoroutine(SwordAttack());
            else StartCoroutine(WhipAttack());

            altAttack = !altAttack;
        }
        attackCooldown = Mathf.Max(attackCooldown - Time.deltaTime, 0);



        if (Input.GetAxisRaw("Fire") != 0) shoot.Pressed();

        if (shootCooldown <= 0)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)transform.position - mousePos;
            shootAim.transform.rotation = Quaternion.LookRotation(transform.forward, -direction);

            direction = (Vector2)gunSR.transform.position - mousePos;
            gunSR.transform.rotation = Quaternion.LookRotation(transform.forward, -direction);

            direction = (Vector2)sniperSR.transform.position - mousePos;
            sniperSR.transform.rotation = Quaternion.LookRotation(transform.forward, -direction);
        }

        if (shootCooldown <= 0 && shoot.GetPress())
        {
            shoot.Unpress();
            if (Player.currentAmmo > 0)
            {
                if (!altFire) StartCoroutine(GunShoot());
                else StartCoroutine(SniperShoot());

                Player.currentAmmo--;
            }
            else
            {
                altFire = !altFire;
                if (!altFire) Player.currentAmmo = Player.gunAmmo;
                else Player.currentAmmo = Player.sniperAmmo;
                shootCooldown = 1.3f;
            }
            
        }
        shootCooldown = Mathf.Max(shootCooldown - Time.deltaTime, 0);

        #endregion
    }

    public IEnumerator WhipAttack()
    {
        whipSR.color = ready;
        attackCooldown = 9999;

        yield return new WaitForSeconds(.1f);
        whipAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.2f);

        swordSR.color = ready;
        attackCooldown = 0.3f;

        yield return new WaitForSeconds(0.3f);

        whipSR.color = notReady;
    }

    public IEnumerator SwordAttack()
    {

        swordSR.color = ready;

        float original = walkSpeed;
        walkSpeed = walkSpeed / 2f;
        attackCooldown = 9999;
        swordAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        whipSR.color = ready;
        walkSpeed = originalWalkSpeed;
        attackCooldown = 0.5f;

        yield return new WaitForSeconds(0.2f);
        swordSR.color = notReady;
    }


    public IEnumerator GunShoot()
    {
        shootCooldown = 9999;
        yield return new WaitForSeconds(0.1f);
        walkSpeed = walkSpeed * 1.2f;

        var bullet = Instantiate(gunBullet);
        bullet.transform.position = gunBarrel.position;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)gunSR.transform.position - mousePos;
        Bullet b = bullet.GetComponent<Bullet>();
        b.SetSpeed(-direction.normalized * gunProjectileSpeed);


        shootCooldown = 0.7f;
        yield return new WaitForSeconds(0.1f);

        walkSpeed = originalWalkSpeed;
    }
    public IEnumerator SniperShoot()
    {
        shootCooldown = 9999;
        attackCooldown = 1f;
        walkSpeed = walkSpeed * 0.3f;
        yield return new WaitForSeconds(0.3f);
        

        var bullet = Instantiate(sniperBullet);
        bullet.transform.position = sniperBarrel.position;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)sniperSR.transform.position - mousePos;
        Bullet b = bullet.GetComponent<Bullet>();
        b.SetSpeed(-direction.normalized * sniperProjectileSpeed);


        shootCooldown = 1f;
        yield return new WaitForSeconds(0.1f);

        walkSpeed = originalWalkSpeed;
    }
    
}
