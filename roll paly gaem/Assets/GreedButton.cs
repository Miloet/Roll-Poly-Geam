using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedButton : MonoBehaviour
{
    public Sprite unactive;
    public Sprite active;

    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = unactive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            this.enabled = false;
            sr.sprite = active;
            Invoke("Return", 1);
            Game.GreedUp();
        }
    }

    void Return()
    {
        this.enabled = true;
        sr.sprite = unactive;
    }
}
