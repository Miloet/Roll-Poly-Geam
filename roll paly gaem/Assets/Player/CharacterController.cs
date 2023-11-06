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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
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

        rb.velocity = input * walkSpeed;
    }
}
