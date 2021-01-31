﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPoint
{
    [SerializeField]
    private int numberOfPoints; // How many points does this sound wave consist of initially

    private SoundWave parent;

    public Vector3 position;

    public Quaternion direction;

    public bool spawnsEcho = false;

    public bool stopped = false;

    public SoundPoint(SoundWave parent, Vector2 position, Quaternion direction, bool spawnsEcho)
    {
        this.parent = parent;
        this.position = position;
        this.direction = direction;
        this.spawnsEcho = spawnsEcho;
    }

    void Stop()
    {
        stopped = true;

        if (spawnsEcho)
            parent.SpawnEcho(this);
    }

    public void Tick(float deltaTime)
    {
        if (!stopped)
        {
            MoveForward(deltaTime);
            CheckForward(deltaTime);
        }
    }

    void MoveForward(float deltaTime)
    {
        position += direction * Vector3.right * deltaTime * SoundWave.WAVE_SPEED;
    }

    public bool CheckForward(float deltaTime)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction * Vector3.right, SoundWave.WAVE_SPEED * deltaTime, parent.checkMask);

        if (hit.collider != null)
        {
            Stop();

            return true;
        }
        else
            return false;
    }
}
