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

    Transform Gun;
    Transform gunBarrel;
    Animator gunAnimator;
    SpriteRenderer gunSR;
    ParticleSystem gunFlash;

    public float gunProjectileSpeed;
    public GameObject gunBullet;
    private GameObject bulletCasing;
    private Transform bulletCasingPlacement;

    Transform Sniper;
    Transform sniperBarrel;
    Animator sniperAnimator;
    SpriteRenderer sniperSR;
    ParticleSystem sniperFlash;

    public float sniperProjectileSpeed;
    public GameObject sniperBullet;
    private GameObject sniperClip;
    private Transform sniperClipPlacement;

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
        Sniper = sniper.transform;
        sniperAnimator = sniper.GetComponent<Animator>();
        sniperSR = sniper.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        sniperBarrel = sniper.transform.Find("Barrel");
        sniperClipPlacement = sniper.transform.Find("ClipPlacement");
        sniperFlash = sniperBarrel.GetComponent<ParticleSystem>();

        


        var gun = shootAim.transform.Find("Gun").gameObject;
        Gun = gun.transform;
        gunAnimator = gun.GetComponent<Animator>();
        gunSR = gun.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        gunBarrel = gun.transform.Find("Barrel");
        bulletCasingPlacement = gun.transform.Find("ClipPlacement");
        gunFlash = gunBarrel.GetComponent<ParticleSystem>();

        gunSR.color = ready;
        sniperSR.color = notReady;

        shoot = Buffer.SetBuffer(gameObject, 0.15f);



        bulletCasing = Resources.Load<GameObject>("Casing");
        sniperClip = Resources.Load<GameObject>("Clip");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Attack") != 0) attack.Pressed();

        #region Movement

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
            Vector2 AmousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 Adirection = (Vector2)transform.position - AmousePos;
            attackAim.transform.rotation = Quaternion.LookRotation(transform.forward, -Adirection);
        }

        if (attackCooldown <= 0 && attack.GetPress())
        {
            if (!altAttack) StartCoroutine(SwordAttack());
            else StartCoroutine(WhipAttack());

            altAttack = !altAttack;
        }
        attackCooldown = Mathf.Max(attackCooldown - Time.deltaTime, 0);



        if (Input.GetAxisRaw("Fire") != 0) shoot.Pressed();


        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)transform.position - mousePos;
        shootAim.transform.rotation = Quaternion.LookRotation(transform.forward, -direction);
        if (!altFire)
        {
            direction = (Vector2)Gun.transform.position - mousePos;
            Gun.transform.rotation = Quaternion.LookRotation(transform.forward, -direction);
            Gun.transform.Rotate(new Vector3(0, 0, 90), Space.Self);

            Transform parent = Gun.transform.parent;
            Gun.transform.position = parent.position + parent.rotation * new Vector3(0.5f, 0);

            Sniper.transform.rotation = Quaternion.Euler(0, 0, 70);
            Sniper.transform.position = transform.position;
        }
        else
        {
            direction = (Vector2)Sniper.transform.position - mousePos;
            Sniper.transform.rotation = Quaternion.LookRotation(transform.forward, -direction);
            Sniper.transform.Rotate(new Vector3(0, 0, 90), Space.Self);

            Transform parent = Gun.transform.parent;
            Sniper.transform.position = parent.position + parent.rotation * new Vector3(-0.5f, 0);
                
            Gun.transform.rotation = Quaternion.Euler(0, 0, 0);
            Gun.transform.position = transform.position;
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
                if (!altFire)
                {
                    Player.currentAmmo = Player.gunAmmo;
                    sniperAnimator.SetTrigger("Empty");
                    gunAnimator.SetTrigger("Reload"); 

                }
                else
                {
                    Player.currentAmmo = Player.sniperAmmo;
                    sniperAnimator.SetTrigger("Reload");
                    gunAnimator.SetTrigger("Empty");
                }
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

        GameObject casing = Instantiate(bulletCasing);
        casing.transform.position = bulletCasingPlacement.position;
        Rigidbody2D rb = casing.GetComponent<Rigidbody2D>();
        rb.velocity = gunSR.transform.rotation * new Vector2(1f, -3f);
        rb.angularVelocity = Random.Range(-360f, 360f);
        Destroy(rb, Random.Range(0.3f, 1f));

        gunAnimator.SetTrigger("Shoot");

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

        if(Player.currentAmmo <= 0)
        {
            GameObject casing = Instantiate(sniperClip);
            casing.transform.position = sniperClipPlacement.position;
            Rigidbody2D rb = casing.GetComponent<Rigidbody2D>();
            rb.velocity = sniperSR.transform.rotation * new Vector2(1f, 3);
            rb.angularVelocity = Random.Range(-360f, 360f);
            Destroy(rb, Random.Range(0.3f, 1f));
        }

        sniperAnimator.SetTrigger("Shoot");


        shootCooldown = 1f;
        yield return new WaitForSeconds(0.1f);

        walkSpeed = originalWalkSpeed;
    }
    
}
