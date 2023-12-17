using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    public TextMeshProUGUI coins;
    public TextMeshProUGUI enemies;
    public TextMeshProUGUI time;


    public GameObject onQuestComplete;

    public static int enemiesKilled;
    public static int enemyQuota;
    public static int coinsCollected;
    public static int coinQuota;

    public static float timeSurvived;
    public static float timeQuota;



    // Update is called once per frame
    void Update()
    {
        if (enemyQuota > 0)
        {
            if (enemiesKilled >= enemyQuota) enemies.text = "Kill Enemies: Done!";
            else enemies.text = $"Kill Enemies: {enemiesKilled} / {enemyQuota}";
        }
        else enemies.text = "";

        if (coinQuota > 0)
        {
            if (coinsCollected >= coinQuota) coins.text = "Collect Coins: Done!";
            else coins.text = $"Collect Coins: {coinsCollected} / {coinQuota}";
        }
        else coins.text = "";

        if (timeQuota > 0)
        {
            if (timeSurvived >= coinQuota) time.text = "Collect Coins: Done!";
            else time.text = $"Collect Coins: {timeSurvived.ToString("0.0")} / {timeQuota}";
        }
        else time.text = "";

        if (coinsCollected >= coinQuota && enemiesKilled >= enemyQuota && timeSurvived >= timeQuota) onQuestComplete.SetActive(false);
    }

    public static void MakeQuest(int enemies = 0, int coins = 0, int time = 0)
    {
        enemyQuota = enemies;
        coinQuota = coins;
        timeQuota = time;

        enemiesKilled = 0;
        coinsCollected = 0;
        timeSurvived = 0;
    }


    public static void ResetProgress()
    {
        enemiesKilled = 0;
        coinsCollected = 0;
        timeSurvived = 0;
    }

}
