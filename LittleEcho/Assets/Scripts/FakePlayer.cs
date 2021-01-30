using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    [SerializeField]
    private float spawnCooldown = 1;
    private float spawnTime;

    [SerializeField]
    private SoundWave prefab;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time - spawnCooldown;
    }

    void InstantiateWave(float duration)
    {
        SoundWave wave = Instantiate(prefab, transform.position, Quaternion.identity);
        wave.Init(duration);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime + spawnCooldown)
        {
            spawnTime = Time.time;

            InstantiateWave(1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            InstantiateWave(5);
        }
    }
}
