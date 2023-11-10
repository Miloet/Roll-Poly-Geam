using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float walkSpeed = 1f;
    public static Vector2 input;

    //Componants

    public static Rigidbody2D rb;
    Animator an;


    bool altAttack;
    float attackCooldown;

    public GameObject aim;

    public GameObject whip;
    public GameObject sword;

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
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.x != 0 || input.y != 0)
        {
            an.SetFloat("Horizontal", input.x);
            an.SetFloat("Vertical", input.y);
        }

        rb.velocity = Vector2.ClampMagnitude(input, 1f) * walkSpeed;

        if (attackCooldown <= 0)
        {
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
        whip.SetActive(true);
        attackCooldown = 0.25f;
        //whipAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.15f);
        whip.SetActive(false);
    }

    public IEnumerator SwordAttack() 
    {
        sword.SetActive(true);

        attackCooldown = 0.5f;
        //swordAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.15f);
        sword.SetActive(false);
    }
    
}
