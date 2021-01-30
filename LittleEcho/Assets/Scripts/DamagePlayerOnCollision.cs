using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    [SerializeField] private int damage = 20;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //only checks for player now

        var obj = collision.gameObject;
        if (obj.tag == "Player")
        {
            obj.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
