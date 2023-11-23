using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int MaxHP = 10;
    [System.NonSerialized] public static int HP;
    [System.NonSerialized] Animator an;

    public static int currentAmmo;
    public int ammo;

    public static int sniperAmmo = 4;
    public static int gunAmmo = 7;


    private void Update()
    {
        ammo = currentAmmo;
    }


    private void Start()
    {
        an = GetComponent<Animator>();
        HP = MaxHP;
        currentAmmo = gunAmmo;
    }
    public void TakeDamage(int dmg)
    {
        HP = Mathf.Clamp(HP - dmg, 0, MaxHP);

        an.SetTrigger("Hit");

        if (HP <= 0) Die();
    }
    public void Die()
    {
        
    }
}
