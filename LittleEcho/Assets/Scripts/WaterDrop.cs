using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    [SerializeField] GameObject soundWave;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject.Instantiate(soundWave);
        this.gameObject.SetActive(false);
        
    }
}
