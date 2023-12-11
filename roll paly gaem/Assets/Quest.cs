using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    public TextMeshProUGUI coins;
    public TextMeshProUGUI enemies;

    public GameObject onQuestComplete;

    public static float enemiesKilled;
    public static float enemyQuota;
    public static float coinsCollected;
    public static float coinQuota;


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

        if(coinsCollected >= coinQuota && enemiesKilled >= enemyQuota) onQuestComplete.SetActive(false);
    }

    public static void MakeQuest(int enemies = 0, int coins = 0)
    {
        enemyQuota = enemies;
        coinQuota = coins;

        enemiesKilled = 0;
        coinsCollected = 0;
    }

}
