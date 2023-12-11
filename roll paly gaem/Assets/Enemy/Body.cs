using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D cl;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();


    }
    void Update()
    {
        if(rb.velocity.magnitude < 0.01f)
        {
            Destroy(rb);Destroy(cl);Destroy(this);
        }
    }
}
