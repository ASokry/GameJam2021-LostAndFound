using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    private SoundWave pingPrefab;

    [SerializeField]
    private float pingCooldown = 10;
    private float pingTimer;

    private void Start()
    {
        pingTimer = pingCooldown;
    }

    private void Update()
    {
        pingTimer += Time.deltaTime;

        if (pingTimer > pingCooldown)
        {
            if (Input.GetButtonDown("Ping"))
            {
                pingTimer = 0;

                Instantiate(pingPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
