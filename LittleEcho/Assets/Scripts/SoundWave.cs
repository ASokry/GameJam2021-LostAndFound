using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public const float WAVE_SPEED = 10;
    public const float ECHO_MULT = 0.5F;

    [SerializeField]
    private int numberOfPoints = 100; // How many points does this sound wave consist of initially

    [SerializeField]
    private float duration = 4;
    private float spawnTime;

    [SerializeField]
    private SoundWave wavePrefab;

    [SerializeField]
    public LayerMask checkMask;

    [SerializeField]
    private List<SoundPoint> points;


    // Start is called before the first frame update
    void Start()
    {
        points = new List<SoundPoint>();
        spawnTime = Time.time;
        SpawnPoints();
    }

    public void Init(float duration)
    {
        this.duration = duration;
        numberOfPoints = PointsPerDuration(duration);
    }

    void SpawnPoints()
    {
        int echoIndex = Random.Range(0, numberOfPoints);
        for (int i = 0; i < numberOfPoints; i++)
        {
            SoundPoint point = new SoundPoint(this, transform.position, Quaternion.Euler(0, 0, i * (360F / numberOfPoints)), i == echoIndex);
            points.Add(point);
        }
    }

    public void SpawnEcho(SoundPoint pointIn)
    {
        if (duration * ECHO_MULT > 0.5) // Echo cutoff
            SpawnSoundWave(pointIn.position, duration * ECHO_MULT);
    }

    void SpawnSoundWave(Vector3 position, float durationIn)
    {
        SoundWave wave = Instantiate(wavePrefab, position, Quaternion.identity);
        wave.Init(durationIn);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime + duration)
        {
            Destroy(gameObject);
        }
        else
        {
            foreach (SoundPoint point in points)
            {
                point.Tick(Time.deltaTime);
            }
        }
    }

    private static int PointsPerDuration(float durationIn)
    {
        return (int)durationIn * 50;
    }
}
