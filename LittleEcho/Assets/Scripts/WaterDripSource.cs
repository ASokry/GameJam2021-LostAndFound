using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDripSource : MonoBehaviour
{
    [SerializeField] int dripFrequency = 2;

    private void Start()
    {
        InvokeRepeating("DripWater", 0, dripFrequency);
    }

    public void DripWater()
    {
        var waterDrop = ObjectPool.SharedInstance.GetPooledObject();
        if (waterDrop != null)
        {
            waterDrop.transform.position = this.transform.position;
            waterDrop.SetActive(true);
        }
    }


}
