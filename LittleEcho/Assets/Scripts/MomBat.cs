using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomBat : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //only checks for player now
        var obj = collision.gameObject;
        if (obj.tag == "Player")
        {
            GameManager.Instance.LoadScene("WinMenu", GameManager.GameState.GameOver);
        }
    }
}
