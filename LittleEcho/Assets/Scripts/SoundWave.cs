using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public const float WAVE_SPEED = 5;
    public const float ECHO_MULT = 0.4F;
    public const float MINIMUM_DURATION = 0.4F;

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
        for (int i = 0; i < numberOfPoints; i++)
        {
            SoundPoint point = new SoundPoint(this, transform.position, Quaternion.Euler(0, 0, i * (360F / numberOfPoints)), false);
            points.Add(point);
        }

        // Assign echo status to some points
        int numberOfEchos = EchosPerPoints(numberOfPoints);
        int echoPoint = Random.Range(0, numberOfPoints);

        HashSet<int> takenPoints = new HashSet<int>();

        // Try a percentage of the points
        for (int i = 0; i < numberOfEchos; i++)
        {
            int counter = 10000;

            // Keep trying until we have a new point
            while (takenPoints.Contains(echoPoint))
            {
                echoPoint = (echoPoint + Random.Range(1, numberOfPoints)) % numberOfPoints;

                // Shouldn't happen
                counter--;
                if (counter < 0)
                {
                    break;
                }
            }

            takenPoints.Add(echoPoint);

            points[echoPoint].spawnsEcho = true;
        }
    }

    public void SpawnEcho(SoundPoint pointIn)
    {
        //if (duration * ECHO_MULT > MINIMUM_DURATION) // Echo cutoff
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
            // Update points
            for (int i = 0; i < points.Count; i++)
            {
                points[i].Tick(Time.deltaTime);
            }

            // Draw edge
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 dir = 0.5F * (i < points.Count - 1 ? points[i].position - points[i + 1].position : points[i].position - points[0].position);
                if (dir.magnitude < 0.5F)
                    Debug.DrawRay(points[i].position - dir, 2 * dir, Color.white, Time.deltaTime);
            }
        }
    }

    private static int PointsPerDuration(float durationIn)
    {
        return (int)durationIn * 25;
    }

    private static int EchosPerPoints(int points)
    {
        return Mathf.Max(0, (int)(points * 0.1F) - 4);
    }
}
