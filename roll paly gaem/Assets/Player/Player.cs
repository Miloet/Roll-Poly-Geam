using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxHP = 10;
    [System.NonSerialized] public int HP;
    [System.NonSerialized] Animator an;


    private void Start()
    {
        an = GetComponent<Animator>();
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
