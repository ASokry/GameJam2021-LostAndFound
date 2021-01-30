using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int health;
    private int maxHealth = 100;

    public HealthBar healthBar;

    private void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <=0)
        {
            // do something
        }
    }

    public void Heal(int health)
    {
        this.health += health;

        if (this.health >= 100)
            this.health = 100;

        healthBar.SetHealth(health);
    }
}
