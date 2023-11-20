using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float walkSpeed = 1f;
    public static Vector2 movementInput;

    //Componants

    public static Rigidbody2D rb;
    Animator an;

    Buffer attack; 
    bool altAttack;
    float attackCooldown;

    GameObject aim;

    GameObject whip;
    Animator whipAnimator;
    SpriteRenderer whipSR;
    GameObject sword;
    Animator swordAnimator;
    SpriteRenderer swordSR;

    Color SwordDefaultColor = new Color(1, 1, 1, 1);
    Color WhipDefaultColor = new Color(1, 1, 1, 1);

    Color SwordNotReady = new Color(0.8f, 0.8f, 0.8f, 0.8f);
    Color WhipNotReady = new Color(0.8f, 0.8f, 0.8f, 0.8f);

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();

        aim = transform.Find("Aim").gameObject;

        sword = aim.transform.Find("Sword").gameObject;
        swordAnimator = sword.GetComponent<Animator>();
        swordSR = sword.GetComponent<SpriteRenderer>();

        whip = aim.transform.Find("Whip").gameObject;
        whipAnimator = whip.GetComponent<Animator>();
        whipSR = whip.GetComponent<SpriteRenderer>();

        whipSR.color = WhipNotReady;
        swordSR.color = SwordDefaultColor;

        attack = Buffer.SetBuffer(gameObject,0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Attack") != 0) attack.Pressed();

        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));



        if (movementInput.x != 0 || movementInput.y != 0)
        {
            an.SetFloat("Horizontal", movementInput.x);
            an.SetFloat("Vertical", movementInput.y);
        }

        rb.velocity = Vector2.ClampMagnitude(movementInput, 1f) * walkSpeed;

        if (attackCooldown <= 0)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)transform.position - mousePos;
            aim.transform.rotation = Quaternion.LookRotation(transform.forward, -direction);
        }

        if (attackCooldown <= 0 && attack.GetPress())
        {
            attack.Unpress();

            if (!altAttack) StartCoroutine(SwordAttack());
            else StartCoroutine(WhipAttack());

            altAttack = !altAttack;
        }
        attackCooldown = Mathf.Max(attackCooldown - Time.deltaTime, 0);
    }

    public IEnumerator WhipAttack() 
    {
        whipSR.color = WhipDefaultColor;
        attackCooldown = 9999;

        yield return new WaitForSeconds(.1f);
        whipAnimator.SetTrigger("Attack");
        
        yield return new WaitForSeconds(0.2f);

        swordSR.color = SwordDefaultColor;
        attackCooldown = 0.3f;

        yield return new WaitForSeconds(0.3f);

        whipSR.color = WhipNotReady;
    }

    public IEnumerator SwordAttack() 
    {
        
        swordSR.color = SwordDefaultColor;

        float original = walkSpeed;
        walkSpeed = walkSpeed / 2f;
        attackCooldown = 9999;
        swordAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        whipSR.color = WhipDefaultColor;
        walkSpeed = original;
        attackCooldown = 0.5f;

        yield return new WaitForSeconds(0.2f);
        swordSR.color = SwordNotReady;
    }
    
}
