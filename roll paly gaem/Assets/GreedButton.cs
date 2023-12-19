using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedButton : MonoBehaviour
{
    public Sprite unactive;
    public Sprite active;

    public bool greed;
    public bool spawn;
    public bool startPassiveSpawn;
    public static bool begin;


    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = unactive;
        begin = false;
    }
    private void Update()
    {
        if(begin)
        {
            Quest.timeSurvived += Time.deltaTime * Game.greed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (startPassiveSpawn && !begin) FindObjectOfType<Game>().PassiveSpawn = true;
            begin = true;
            enabled = false;
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
