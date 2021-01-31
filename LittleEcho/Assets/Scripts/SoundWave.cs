using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SoundWave : MonoBehaviour
{
    public const float WAVE_SPEED = 2;
    public const float ECHO_MULT = 0.5F;
    public const float MINIMUM_DURATION = 0.5F;
    public const int MAXIMUM_ITERATION = 3;

    private int numberOfPoints = 100; // How many points does this sound wave consist of initially

    [SerializeField]
    private float duration = 4;
    private float timeAlive = 0;

    private int iteration;

    [SerializeField]
    private AudioClip normalSound;
    [SerializeField]
    private AudioClip echoSound;

    [SerializeField]
    private SoundWave echoPrefab;

    [SerializeField]
    public LayerMask checkMask;

    [SerializeField]
    private List<SoundPoint> points;

    [SerializeField]
    private AudioSource source;

    private VectorLine line;

    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!initialized)
            Init(duration, 0);
        source.Play();

        points = new List<SoundPoint>();
        SpawnPoints();
    }

    private void Init(float duration, int iteration)
    {
        initialized = true;

        Debug.Log(iteration);

        line = new VectorLine(name, new List<Vector3>(), 4F);
        //line.drawTransform.SetParent(transform);

        this.duration = duration;
        numberOfPoints = PointsPerDuration(duration);

        this.iteration = iteration;
        source.volume *= AudioVolumePerIteration(iteration);
        source.pitch *= AudioPitchPerIteration(iteration);

        source.clip = iteration == 0 ? normalSound : echoSound;
    }

    public void Init(float duration)
    {
        Init(duration, 0);
    }

    void SpawnPoints()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            SoundPoint point = new SoundPoint(this, transform.position, Quaternion.Euler(0, 0, i * (360F / numberOfPoints)), false);

            if (!point.CheckForward(0.1F)) // Too close to a wall?
            {
                points.Add(point);

                // Cache two positions per point
                line.points3.Add(point.position);
                line.points3.Add(point.position);
            }
        }

        if (points.Count > 0)
        {
            // Assign echo status to some points
            int numberOfEchos = EchosPerDurationIn(duration);
            int echoPoint = Random.Range(0, points.Count);

            HashSet<int> takenPoints = new HashSet<int>();


            // Try a percentage of the points
            for (int i = 0; i < numberOfEchos; i++)
            {
                int counter = 10000;

                // Keep trying until we have a new point
                while (takenPoints.Contains(echoPoint))
                {
                    echoPoint = (echoPoint + Random.Range(1, points.Count)) % points.Count;

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
    }

    public void SpawnEcho(SoundPoint pointIn)
    {
        if (duration * ECHO_MULT >= MINIMUM_DURATION && iteration + 1 <= MAXIMUM_ITERATION)
            SpawnSoundWave(pointIn.position, duration * ECHO_MULT, iteration + 1);
    }

    void SpawnSoundWave(Vector3 position, float durationIn, int iterationIn)
    {
        SoundWave wave = Instantiate(echoPrefab, position, Quaternion.identity);
        wave.Init(durationIn, iterationIn);
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        timeAlive += deltaTime;

        if (timeAlive > duration)
        {
            End();
        }
        else
        {
            // Update points
            for (int i = 0; i < points.Count; i++)
            {
                points[i].Tick(deltaTime);
            }

            // Draw edge
            if (points.Count > 1)
            {
                Color color = iteration == 0 ? Color.yellow : iteration == 1 ? Color.red : iteration == 2 ? Color.magenta : Color.blue;

                float alpha = timeAlive / duration;
                alpha = 1 - alpha * alpha;
                color = new Color(color.r, color.g, color.b, color.a * alpha);

                line.color = color;


                for (int i = 0; i < points.Count; i++)
                {
                    SoundPoint pointB = i < points.Count - 1 ? points[i + 1] : points[0];

                    Vector3 dir = -(points[i].position - pointB.position);

                    float arcLength = 2 * Mathf.PI * (timeAlive * WAVE_SPEED) / numberOfPoints;
                    bool stopped = points[i].stopped || pointB.stopped;
                    bool shortEnough = dir.magnitude <= arcLength + 0.1F || (stopped && dir.magnitude <= 1);

                    RaycastHit2D hit = Physics2D.Raycast(points[i].position, dir, dir.magnitude, checkMask);
                    if (shortEnough && hit.collider == null)
                    {
                        line.points3[2 * i] = points[i].position;
                        line.points3[2 * i + 1] = pointB.position;

                        //Debug.DrawRay(points[i].position, dir, color, deltaTime);
                    }
                }

                line.Draw();
            }
        }
    }

    private void End()
    {
        points.Clear();
        VectorLine.Destroy(ref line);

        if (source.clip && timeAlive > source.clip.length)
            Destroy(gameObject);
    }

    private static int PointsPerDuration(float durationIn)
    {
        return (int)(durationIn * 30);
    }

    private static int EchosPerDurationIn(float durationIn)
    {
        return Mathf.Max(0, (int)(Mathf.Log(durationIn) + (durationIn + 1) / 2F - 1.5F));
    }

    private static float AudioVolumePerIteration(int iterationIn)
    {
        return Mathf.Pow(0.4F, iterationIn) * iterationIn > 0 ? 0.4F : 1;
    }

    private static float AudioPitchPerIteration(int iterationIn)
    {
        return Mathf.Pow(0.9F, Mathf.Max(0, iterationIn - 1));
    }

    public int GetNumberOfPoints()
    {
        return numberOfPoints;
    }
}
