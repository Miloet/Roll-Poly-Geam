using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    float force = 3;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-force, force), Random.Range(-force, force)).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Quest.coinsCollected++;
            Destroy(gameObject);
        }
    }

}
