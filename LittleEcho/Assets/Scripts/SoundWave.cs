using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SoundWave : MonoBehaviour
{
    // Constants
    public const float WAVE_SPEED = 2; // Speed at which wave expands
    public const float ECHO_MULT = 0.5F; // Duration multiplier applied to each echo
    public const int MAXIMUM_ITERATION = 3; // How many recursive echos are allowed

    // Has the Init() method been called yet?
    private bool initialized = false;

    // Max number of points that this sound wave consists of
    private int numberOfPoints = 100;

    [Header("General Settings")]
    // How long this sound wave lasts
    [SerializeField]
    private float duration = 4;
    private float timeAlive = 0;

    // What layers count as "walls." Should exclude the sound source itself
    [SerializeField]
    public LayerMask checkMask;

    // Sound wave template for echos
    [SerializeField]
    private SoundWave echoPrefab = null;

    // Starts at 0. Each echo increases the iteration by 1
    private int iteration;

    [Header("Audio Settings")]
    // Audio to play when spawned or echod
    [SerializeField]
    private AudioClip normalSound = null;
    [SerializeField]
    private AudioClip echoSound = null;

    // Representation of the sound wave
    private List<SoundPoint> points;
    private VectorLine line;

    // Cached and used to play sounds
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize properties
        if (!initialized)
            Init(duration, 0);

        // Play the source now that it has been intialized
        source.Play();

        // Create a VectorLine for rendering
        line = new VectorLine(name, new List<Vector3>(), 4F);

        // Based on the number of sound points we need, spawn them in
        points = new List<SoundPoint>();
        SpawnPoints();
    }

    /// <summary>
    /// Performs any setup necessary, given certain properties that are passed in
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="iteration"></param>
    private void Init(float duration, int iteration)
    {
        initialized = true; // Prevent this from running multiple times (esp in Start())

        // Cache components
        source = GetComponent<AudioSource>();

        // Set up properties tied to duration (the number of sound points)
        this.duration = duration;
        numberOfPoints = PointsPerDuration(duration);

        // Set up properties tied to iteration (audio related)
        this.iteration = iteration;
        source.volume *= AudioVolumePerIteration(iteration);
        source.pitch *= AudioPitchPerIteration(iteration);
        source.clip = iteration == 0 ? normalSound : echoSound;
    }

    public void Init(float duration)
    {
        Init(duration, 0);
    }

    /// <summary>
    /// Generates sound points and adds them to the list
    /// </summary>
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

        // Are there any points to iterate through?
        if (points.Count > 0)
        {
            // Make some of the points "echo points"
            int numberOfEchos = EchosPerDurationIn(duration);
            int echoPoint = Random.Range(0, points.Count);

            HashSet<int> takenPoints = new HashSet<int>();

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
        if (iteration + 1 <= MAXIMUM_ITERATION)
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

        // Should the soundwave be destroyed
        if (timeAlive <= duration)
        {
            // Simulate points
            for (int i = 0; i < points.Count; i++)
                points[i].Tick(deltaTime);

            // Display points
            if (points.Count > 1)
                RenderPoints();
        }
        else
            Expired();
    }

    /// <summary>
    /// Called when the duration of this sound wave expires
    /// </summary>
    private void Expired()
    {
        points.Clear();
        VectorLine.Destroy(ref line);

        if (source.clip && timeAlive > source.clip.length)
            Destroy(gameObject);
    }

    /// <summary>
    /// Draws a vector line through each point
    /// </summary>
    private void RenderPoints()
    {
        // Fade color over time
        Color color = iteration == 0 ? Color.yellow : iteration == 1 ? Color.red : iteration == 2 ? Color.magenta : Color.blue;

        float alpha = timeAlive / duration;
        alpha = 1 - alpha * alpha;
        color = new Color(color.r, color.g, color.b, color.a * alpha);

        line.color = color;

        // Iterate through all points and connect each one to its adjacent neighbor
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
            }
        }

        line.Draw();
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
