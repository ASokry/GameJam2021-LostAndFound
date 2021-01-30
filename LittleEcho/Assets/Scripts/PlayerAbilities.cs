using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    private SoundWave pingPrefab;

    private void Update()
    {
        if (Input.GetButtonDown("Ping"))
        {
            Instantiate(pingPrefab, transform.position, Quaternion.identity);
        }
    }
}
