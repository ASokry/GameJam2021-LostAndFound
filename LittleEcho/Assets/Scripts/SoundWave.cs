using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public const float waveSpeed = 4;

    [SerializeField]
    private int numberOfPoints; // How many points does this sound wave consist of initially

    [SerializeField]
    private float duration;

    [SerializeField]
    private SoundPoint prefab;

    [SerializeField]
    private List<SoundPoint> points;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("InstantiatePoints", 0.5F);
    }

    void InstantiatePoints()
    {
        int num = 100;
        for (int i = 0; i < num; i++)
        {
            SoundPoint point = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, i * (360F / num)));
            points.Add(point);
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (SoundPoint point in points)
            point.Tick(Time.deltaTime);
    }
}
;