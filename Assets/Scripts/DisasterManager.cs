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
    void Start()
    {
        StartCoroutine(RoutineDisaster());
    }

    IEnumerator RoutineDisaster ()
    {
        //while (true)
        {
            var obj = Instantiate(prefabsDisaster[Random.Range(0, prefabsDisaster.Length)]); 
            //var disaster = obj.GetComponent<Disaster>();
            Vector3 point = Random.onUnitSphere * (planet.GetRadius());
            obj.transform.position = point;
            obj.transform.up = (point - planet.transform.position).normalized;
            yield return new WaitForSeconds(spawnTime + Random.Range(-spawnTimeRange, spawnTimeRange));
        }
    }
}
