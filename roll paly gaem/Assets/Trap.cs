using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject exit;

    private void Start()
    {
        exit.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            exit.SetActive(true);
            Destroy(gameObject);
        }
    }
}
