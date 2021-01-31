using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDripSource : MonoBehaviour
{
    [SerializeField] float dripFrequency = 2;
    [SerializeField] ObjectPool waterDropPool;
    float totalTime;
    float coolDown;

    private void Start()
    {
        dripFrequency += Random.Range(.1f, .5f);
        totalTime = 0;
        coolDown = dripFrequency;
        StartCoroutine(DripWater(dripFrequency));
    }

    IEnumerator DripWater(float frequency)
    {

        yield return new WaitUntil(() =>
        {
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) return true;

            if (totalTime - coolDown >= dripFrequency
            && GameManager.Instance.CurrentGameState != GameManager.GameState.Paused)
            {
                coolDown = totalTime;

                var waterDrop = waterDropPool.GetPooledObject();
                if (waterDrop != null)
                {
                    waterDrop.transform.position = this.transform.position;
                    waterDrop.SetActive(true);
                }
            }

            totalTime += Time.deltaTime;

            return false;

        });
    }



}
