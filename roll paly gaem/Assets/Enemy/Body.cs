using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D cl;

    public GameObject coin;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();

        Quest.enemiesKilled++;
    }
    void Update()
    {
        if(rb.velocity.magnitude < 0.01f)
        {
            Destroy(rb);Destroy(cl);Destroy(this);
            for(int i = 0; i < 5; i += Random.Range(0,3))
                Instantiate(coin).transform.position = transform.position;
        }
    }
}
