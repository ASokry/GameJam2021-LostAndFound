using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    [SerializeField]
    private float spawnCooldown = 1;
    private float spawnTime;

    [SerializeField]
    private SoundWave flapPrefab = null;

    [SerializeField]
    private SoundWave pingPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime + spawnCooldown)
        {
            spawnTime = Time.time;

            Instantiate(flapPrefab, transform.position, Quaternion.identity);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Instantiate(pingPrefab, transform.position, Quaternion.identity);
        }
    }
}
