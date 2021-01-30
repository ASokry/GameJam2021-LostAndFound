using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPoint : MonoBehaviour
{
    [SerializeField]
    private LayerMask checkMask;

    [SerializeField]
    private int numberOfPoints; // How many points does this sound wave consist of initially

    [SerializeField]
    private float duration;

    private bool stopped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Stop()
    {
        stopped = true;
    }

    public void Tick(float deltaTime)
    {
        if (stopped)
            return;

        MoveForward(deltaTime);
        CheckForward(deltaTime);
    }

    void MoveForward(float deltaTime)
    {
        transform.position += transform.right * deltaTime * SoundWave.waveSpeed;
    }

    void CheckForward(float deltaTime)
    {
        Debug.DrawRay(transform.position, transform.right * SoundWave.waveSpeed * deltaTime, Color.white, 0.1F);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, SoundWave.waveSpeed * deltaTime, checkMask);

        if (hit.collider != null)
        {
            Stop();

            Debug.Log("hit");
            for (int i = 0; i < 5; i++)
            {
                Debug.DrawRay(hit.point, Random.insideUnitSphere, Color.blue, Random.value * 0.4F);
            }
        }
    }
}
