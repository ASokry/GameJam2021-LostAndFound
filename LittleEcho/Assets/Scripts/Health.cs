using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health;
    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private float invulnDuration = 0.5F;
    private float invulnTimer;

    public HealthBar healthBar;

    public SoundWave damageSoundWave;

    private void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        invulnTimer -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (invulnTimer > 0)
            return;
        invulnTimer = invulnDuration;

        health -= damage;
        healthBar.SetHealth(health);

        Instantiate(damageSoundWave, transform.position, Quaternion.identity);

        if (health <= 0)
        {
            // Do something
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver, 1);
        }
    }

    public void Heal(int health)
    {
        this.health += health;

        if (this.health >= maxHealth)
            this.health = maxHealth;

        healthBar.SetHealth(health);
    }
}
