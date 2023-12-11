using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedButton : MonoBehaviour
{
    public Sprite unactive;
    public Sprite active;

    public bool greed;
    public bool spawn;

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
            if (greed) Game.GreedUp();
            if(spawn) Game.Spawn(Game.Enemy.Shooter,3 + Game.greed);
        }
    }

    void Return()
    {
        this.enabled = true;
        sr.sprite = unactive;
    }
}
