using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDripSource : MonoBehaviour
{
    [SerializeField] float dripFrequency = 2;
    float totalTime;
    float coolDown;

    private void Start()
    {
        dripFrequency *= Random.Range(.1f, .5f);
        StartCoroutine(DripWater(dripFrequency));
    }

    IEnumerator DripWater(float frequency)
    {
        coolDown = dripFrequency;

        yield return new WaitUntil(() =>
        {

            if(GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) return true;

            if(totalTime - coolDown <= dripFrequency
            && GameManager.Instance.CurrentGameState != GameManager.GameState.Paused)
            {
                coolDown = totalTime;

                var waterDrop = ObjectPool.SharedInstance.GetPooledObject();
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
