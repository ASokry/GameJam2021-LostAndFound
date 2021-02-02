using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    [SerializeField] GameObject soundWave = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var wave = GameObject.Instantiate(soundWave);
        wave.transform.position = this.transform.position;
        this.gameObject.SetActive(false);
        
    }
}
