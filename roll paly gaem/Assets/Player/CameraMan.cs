using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public float speed;
    Transform player;
    Rigidbody2D playerRB;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = player.position - transform.position;
        transform.position = transform.position + (Vector3)(direction * speed) * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
