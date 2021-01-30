using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    [SerializeField]
    private float spawnCooldown = 1;
    private float spawnTime;

    [SerializeField]
    private float flapVolume = 1;

    [SerializeField]
    private float screamVolume = 5;

    [SerializeField]
    private SoundWave prefab;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
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

            InstantiateWave(flapVolume);
        }

        if (Input.GetButtonDown("Jump"))
        {
            InstantiateWave(screamVolume);
        }
    }
}
