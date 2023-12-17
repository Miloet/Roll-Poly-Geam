using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartQuest : MonoBehaviour
{

    public enum Reward
    {
        None,
        SpeedUp,
        AmmoUp,
        HpUp
    }

    public Reward reward = Reward.None;

    public int enemy = 10;
    public int coin = 30;
    public int time = 0;

    public GameObject note;
    public GameObject quest;

    private void Start()
    {
        note.SetActive(false);
        quest.SetActive(false);

        switch(reward)
        {
            case Reward.SpeedUp:
                CharacterController.originalWalkSpeed += 1;
                break;
            case Reward.AmmoUp:
                Player.gunAmmo += 1;
                Player.sniperAmmo += 1;
                break;
            case Reward.HpUp:
                Player.MaxHP += 2;
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            note.SetActive(true);
            Invoke("End",10);
        }
    }

    public void End()
    {
        Destroy(gameObject);
        Destroy(note);
        quest.SetActive(true);

        Quest.MakeQuest(enemy, coin, time);
    }
}
