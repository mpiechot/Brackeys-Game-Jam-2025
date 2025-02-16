using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 50;

    private int currentHealth;

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("DIE");
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

}
