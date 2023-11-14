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
    GameObject sword;

    

    private Camera cam;

    


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();

        aim = transform.Find("Aim").gameObject;
        sword = aim.transform.Find("Sword").gameObject;
        whip = aim.transform.Find("Whip").gameObject;


        attack = Buffer.SetBuffer(gameObject,0.15f);
    }

    // Update is called once per frame
    void Update()
    {


        if(Input.GetAxisRaw("Attack") != 0) attack.Pressed();

        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));



        if (movementInput.x != 0 || movementInput.y != 0)
        {
            an.SetFloat("Horizontal", movementInput.x);
            an.SetFloat("Vertical", movementInput.y);
        }

        rb.velocity = Vector2.ClampMagnitude(movementInput, 1f) * walkSpeed;

        if (attackCooldown <= 0 && attack.GetPress())
        {
            attack.Unpress();

            if (!altAttack) StartCoroutine(SwordAttack());
            else StartCoroutine(WhipAttack());
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)transform.position - mousePos;
            aim.transform.rotation = Quaternion.LookRotation(transform.forward, -direction);
            altAttack = !altAttack;
        }
        attackCooldown = Mathf.Max(attackCooldown - Time.deltaTime, 0);


        

    }

    public IEnumerator WhipAttack() 
    {
        attackCooldown = 9999;
        yield return new WaitForSeconds(0.10f);
        whip.SetActive(true);
        
        //whipAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.15f);
        attackCooldown = 0.25f;
        whip.SetActive(false);
    }

    public IEnumerator SwordAttack() 
    {
        float original = walkSpeed;
        walkSpeed = walkSpeed / 2f;
        attackCooldown = 9999;


        yield return new WaitForSeconds(0.35f);

        sword.SetActive(true);
        //swordAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.4f);

        walkSpeed = original;
        attackCooldown = 0.5f;
        sword.SetActive(false);
    }
    
}
