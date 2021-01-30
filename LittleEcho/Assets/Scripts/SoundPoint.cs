using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPoint
{
    [SerializeField]
    private int numberOfPoints; // How many points does this sound wave consist of initially

    [SerializeField]
    private float duration;

    private SoundWave parent;

    private Vector3 position;

    private Quaternion direction;

    private bool stopped = false;

    public SoundPoint(SoundWave parent, Vector2 position, Quaternion direction)
    {
        this.parent = parent;
        this.position = position;
        this.direction = direction;
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
        position += direction * Vector3.right * deltaTime * SoundWave.waveSpeed;
    }

    void CheckForward(float deltaTime)
    {
        Debug.DrawRay(position, direction * Vector3.right * SoundWave.waveSpeed * deltaTime, Color.white, 0.1F);

        RaycastHit2D hit = Physics2D.Raycast(position, direction * Vector3.right, SoundWave.waveSpeed * deltaTime, parent.checkMask);

        if (hit.collider != null)
        {
            Stop();

            for (int i = 0; i < 5; i++)
            {
                Debug.DrawRay(hit.point, Random.insideUnitSphere, Color.blue, Random.value * 1.4F);
            }
        }
    }
}
