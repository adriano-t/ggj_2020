using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public float moveRange = 10;

    public GameObject[] prefabsDisasterGround;
    public GameObject[] prefabsDisasterSky;


    public class DisasterStatus
    {
        public GameObject disaster;
        public Cell cell;
        public int index;

        public void CellSet (Cell c) { cell = c; }
    }

    void Start ()
    {
        StartCoroutine(RoutineDisasterGround());
        StartCoroutine(RoutineDisasterSky());
    }

    IEnumerator RoutineDisasterGround ()
    {
        yield return new WaitForSeconds(0.1f);
        List<DisasterStatus> activeDisasters = new List<DisasterStatus>();

        while (true)
        {
            Cell cell = MAIN.GetGlobal().FindFreeCell();

            if (cell != null)
            {
                // chance 
                if (Random.Range(0, activeDisasters.Count * 2) == 0 && activeDisasters.Count < MAIN.GetGlobal().difficulty + 2)
                {
                    int disasterType = Random.Range(0, prefabsDisasterGround.Length);
                    GameObject obj = Instantiate(prefabsDisasterGround[disasterType]);
                    obj.transform.position = cell.transform.position;
                    MAIN.Orient(obj.transform);
                    activeDisasters.Add(new DisasterStatus() { disaster = obj, cell = cell, index = disasterType });
                }
            }

            yield return new WaitForSeconds(Random.Range(2, moveRange + 2));


            for (int i = 0; i < activeDisasters.Count; i++)
            {
                if (activeDisasters[i].disaster == null)
                {
                    activeDisasters.RemoveAt(i);
                    i--;
                    continue;
                }

                yield return new WaitForSeconds(1);

                if (activeDisasters[i].index == 0) activeDisasters[i].cell.SetFire();

                List<Cell> neigh = activeDisasters[i].cell.GetNeighbors();

                if (neigh.Count > 0)
                {
                    MAIN.Shuffle(neigh, -1);
                    Cell n1 = neigh[0];
                    Cell target = null;

                    while (target == null && neigh.Count > 0)
                    {
                        target = neigh[0];
                        if (target && !target.IsSuitableForThunderEvent()) target = null;
                        neigh.RemoveAt(0);
                    }

                    if (!target)
                    {
                        target = n1;
                    }

                    if (target)
                    {
                        activeDisasters[i].CellSet(target);
                        Vector3 startPos = activeDisasters[i].disaster.transform.position;

                        for (float j = 0; j < 1; j += Time.deltaTime)
                        {
                            activeDisasters[i].disaster.transform.position = Vector3.Lerp(startPos, target.transform.position, j);
                            MAIN.Orient(activeDisasters[i].disaster.transform);
                            yield return null;
                        }
                    }
                }

            }
        }
    }

    IEnumerator RoutineDisasterSky ()
    {
        yield return new WaitForSeconds(0.1f);
        List<DisasterStatus> activeDisasters = new List<DisasterStatus>();

        //yield return new WaitForSeconds(20.0f);
        while (true)
        {
            Cell cell = MAIN.GetGlobal().FindFreeCell();

            Vector3 center = MAIN.GetGlobal().GetActivePlanet().GetCenter();
            Ray ray = new Ray(center, MAIN.GetDir(center, cell.transform.position));

            Vector3 point = ray.GetPoint(MAIN.GetGlobal().GetActivePlanet().GetRadius() * 10) + Random.onUnitSphere * 6;
            GameObject go = Instantiate(prefabsDisasterSky[Random.Range(0, prefabsDisasterSky.Length)], point, Quaternion.identity);
            
            for (float i = 0; i < 1; i+=Time.deltaTime * 0.25f)
            {
                go.transform.position = Vector3.Lerp(point, cell.transform.position, i*i);
                //todo rotate 
                yield return null;
            }
            MAIN.SoundPlay(MAIN.GetGlobal().sounds, "asteroide", cell.transform.position);
            //impact
            //todo particles
            cell.SetStato(Cell.Stato.ghiaccio);
            Destroy(go);

            //yield return new WaitForSeconds(Random.Range(20, 60));
        }
    }
}