using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartQuest : MonoBehaviour
{
    public GameObject note;
    public GameObject quest;

    private void Start()
    {
        note.SetActive(false);
        quest.SetActive(false);
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

        Quest.MakeQuest(10, 30);
    }
}
