using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPoint
{
    // The sound wave that manages this point
    private SoundWave parent;

    // Emulate Transform data
    public Vector3 position;
    public Quaternion direction;

    // Marked at spawn time to either trigger an echo or not
    public bool spawnsEcho = false;

    // Has this sound point hit something and stopped simulating
    public bool stopped = false;

    public SoundPoint(SoundWave parent, Vector2 position, Quaternion direction, bool spawnsEcho)
    {
        this.parent = parent;
        this.position = position;
        this.direction = direction;
        this.spawnsEcho = spawnsEcho;
    }

    /// <summary>
    /// Called when this sound point hits something
    /// </summary>
    void Stop()
    {
        stopped = true;

        if (spawnsEcho)
            parent.SpawnEcho(this);
    }

    /// <summary>
    /// Simulates this sound point for one time step. Usually called in an "Update" function
    /// </summary>
    /// <param name="timeStep"></param>
    public void Tick(float timeStep)
    {
        if (!stopped)
        {
            MoveForward(timeStep);

            if (CheckForward(timeStep))
                Stop();
        }
    }

    /// <summary>
    /// Advances this sound point forward along its path, given a time step
    /// </summary>
    /// <param name="timeStep"></param>
    void MoveForward(float timeStep)
    {
        position += direction * Vector3.right * timeStep * SoundWave.WAVE_SPEED;
    }

    /// <summary>
    /// Looks ahead along this sound point's path, given a time step, and returns if it hit something
    /// </summary>
    /// <param name="timeStep"></param>
    public bool CheckForward(float timeStep)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction * Vector3.right, SoundWave.WAVE_SPEED * timeStep, parent.checkMask);

        return hit.collider != null;
    }
}
