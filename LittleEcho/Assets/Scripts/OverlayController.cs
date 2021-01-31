using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayController : MonoBehaviour
{
    private Vector3 min;
    private Vector3 max;
    private float growValue = 1f;
    [Range(0.25f, 0.5f)]
    public float growRate = 0.375f;

    [Range(0, 1.5f)]
    public float maxScalar = 1.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        min = transform.localScale;
        max = transform.localScale * maxScalar;
    }

    private void FixedUpdate()
    {
        // Orginal code (Bug: holding down Vertical buttons will continue to expand the circle)
        //transform.localScale = Vector3.Lerp(min, max, Input.GetAxis("Vertical"));

        // Only decrease value when it is greater than 0 and not inputing
        if (growValue > 0 && !Input.GetButtonDown("Flap"))
        {
            // Decrease lerp value
            growValue -= Time.deltaTime;
        }

        // Constantly changes the scale of the circle by the lerp growValue
        transform.localScale = Vector3.Lerp(min, max, growValue);
    }

    private void Update()
    {
        // Only increase value when inputing
        if (Input.GetButtonDown("Flap"))
        {
            // Increase lerp value
            growValue += growRate;
        }
        
    }
}
