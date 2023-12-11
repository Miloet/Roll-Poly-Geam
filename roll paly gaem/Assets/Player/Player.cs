using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static int MaxHP = 10;
    [System.NonSerialized] public static int HP;
    [System.NonSerialized] Animator an;

    public static int currentAmmo;
    public int ammo;

    private float inv;

    public static int sniperAmmo = 7;
    public static int gunAmmo = 4;

    public GameObject Heart;
    public GameObject UI;

    public Image[] healthUI;


    Sprite FullHeart;
    Sprite EmptyHeart;


    private void Update()
    {
        ammo = currentAmmo;
        if(inv > 0) inv -= Time.deltaTime;
    }


    private void Start()
    {
        an = GetComponent<Animator>();
        HP = MaxHP;
        currentAmmo = gunAmmo;

        FullHeart = Resources.Load<Sprite>("HeartFull");
        EmptyHeart = Resources.Load<Sprite>("HeartEmpty");

        healthUI = new Image[MaxHP];  
        for(int i = 0; i < MaxHP; i++)
        {
            healthUI[i] = Instantiate(Heart, UI.transform).GetComponent<Image>();
            healthUI[i].sprite = FullHeart;
            healthUI[i].transform.position = new Vector2(50f + 32f*2f * i, healthUI[i].transform.position.y);
        }
    }
    public void TakeDamage(int dmg)
    {
        if (inv <= 0)
        {
            inv = 0.5f;
            HP = Mathf.Clamp(HP - dmg, 0, MaxHP);

            UpdateHealth();

            if (HP <= 0)
            {
                Invoke("Die", 0.5f);
                an.SetTrigger("Die");
            }
            else an.SetTrigger("Hit");
        }
    }


    public void UpdateHealth()
    {
        for(int i = 0; i < healthUI.Length; i++)
        {
            if (i < HP) healthUI[i].sprite = FullHeart;
            else healthUI[i].sprite = EmptyHeart;
        }
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
