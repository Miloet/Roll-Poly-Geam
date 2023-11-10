using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxHP = 10;
    [System.NonSerialized] public int HP;


    public void TakeDamage(int dmg)
    {
        HP = Mathf.Clamp(HP - dmg, 0, MaxHP);
        if (HP <= 0) Die();
    }
    public void Die()
    {
        
    }
}
