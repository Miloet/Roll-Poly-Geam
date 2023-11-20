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

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, player.position, Time.deltaTime*speed);
        transform.position = new Vector3(transform.position.x,transform.position.y,-10);
    }
}
