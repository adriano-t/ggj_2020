using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    float difficulty = 1;
    float spawnTime = 60;
    float spawnTimeRange = 10;

    
    void Start()
    {
        
    }

    IEnumerator RoutineDisaster ()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime + Random.Range(-spawnTimeRange, spawnTimeRange));
        }
    }
}
