using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public const float waveSpeed = 14;

    [SerializeField]
    private int numberOfPoints = 100; // How many points does this sound wave consist of initially

    [SerializeField]
    private float duration = 4;
    private float spawnTime;

    [SerializeField]
    private SoundPoint prefab;

    [SerializeField]
    public LayerMask checkMask;

    [SerializeField]
    private List<SoundPoint> points;


    // Start is called before the first frame update
    void Start()
    {
        points = new List<SoundPoint>();
        spawnTime = Time.time;
        Invoke("InstantiatePoints", 0.5F);
    }

    void InstantiatePoints()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            SoundPoint point = new SoundPoint(this, transform.position, Quaternion.Euler(0, 0, i * (360F / numberOfPoints)));
            points.Add(point);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime + duration)
        {
            points.Clear();
        }
        else
        {
            foreach (SoundPoint point in points)
            {
                point.Tick(Time.deltaTime);
            }
        }
    }
}
;