using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public float difficulty = 1;
    public float spawnTime = 60;
    public float spawnTimeRange = 10;
    public GameObject[] prefabsDisaster;
    public Planet planet;
    public float stormSpawnHeight = 2;
    void Start()
    {
        
    }

    IEnumerator RoutineDisaster ()
    {
        while (true)
        {
            var obj = prefabsDisaster[Random.Range(0, prefabsDisaster.Length)];
            //var disaster = obj.GetComponent<Disaster>();
            Vector3 point = Random.onUnitSphere * (planet.GetRadius() + stormSpawnHeight);
            obj.transform.position = point;
            obj.transform.forward = planet.transform.position - point;
            yield return new WaitForSeconds(spawnTime + Random.Range(-spawnTimeRange, spawnTimeRange));
        }
    }
}
