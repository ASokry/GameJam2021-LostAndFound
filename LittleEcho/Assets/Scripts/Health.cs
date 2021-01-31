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
            // Load lose menu and change state to GameOver
            GameManager.Instance.LoadScene("LoseMenu", GameManager.GameState.GameOver);
            //GameManager.Instance.ChangeState(GameManager.GameState.GameOver, 1);
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
