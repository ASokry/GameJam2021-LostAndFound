using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <=0)
        {
            // do something
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver, 1);
        }
    }

    public void Heal(int health)
    {
        this.health += health;

        if (this.health >= 100)
            this.health = 100;
    }
}
