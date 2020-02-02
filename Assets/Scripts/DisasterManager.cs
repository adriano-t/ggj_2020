using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public float difficulty = 1;
    public float spawnTime = 60;
    public float spawnTimeRange = 10;

    public GameObject[] prefabsDisaster;


    public struct DisasterStatus
    {
        public GameObject disaster;
        public Cell cell;

        public void CellSet (Cell c) { cell = c; }
    }

    void Start()
    {
        StartCoroutine(RoutineDisaster());
    }

    IEnumerator RoutineDisaster ()
    {
        yield return new WaitForSeconds(0.1f);
        List<DisasterStatus> activeDisasters = new List<DisasterStatus>();

        while (true)
        {
            Cell cell = MAIN.GetGlobal().FindFreeCell();

            if (cell != null)
            {
                // chance 
                if (Random.Range(0, activeDisasters.Count) == 0)
                {
                    GameObject obj = Instantiate(prefabsDisaster[Random.Range(0, prefabsDisaster.Length)]);
                    obj.transform.position = cell.transform.position;
                    MAIN.Orient(obj.transform);
                    activeDisasters.Add(new DisasterStatus() { disaster = obj, cell = cell });
                }
            }
            else
            {
                yield return new WaitForSeconds(5);
            }

            
            yield return new WaitForSeconds(spawnTime + Random.Range(-spawnTimeRange, spawnTimeRange));

            for (int i=0; i<activeDisasters.Count; i++)
            {
                if (activeDisasters[i].disaster == null)
                {
                    activeDisasters.RemoveAt(i);
                    i--;
                    continue;
                }

                List<Cell> neigh = activeDisasters[i].cell.GetNeighbors();

                if (neigh.Count > 0)
                {
                    MAIN.Shuffle(neigh, -1);
                    Cell target = null;

                    while (target == null && neigh.Count > 0)
                    {
                        target = neigh[0];
                        if (!target.IsSuitableForThunderEvent()) target = null;
                        neigh.RemoveAt(0);
                    }

                    if(target)
                    {
                        activeDisasters[i].CellSet(target);
                        Vector3 startPos = activeDisasters[i].disaster.transform.position;

                        for (float j = 0; j < 1; j += Time.deltaTime)
                        {
                            activeDisasters[i].disaster.transform.position = Vector3.Lerp(startPos, target.transform.position, j);
                            yield return null;
                        }
                    }
                }

            }
        }
    }

    

}
