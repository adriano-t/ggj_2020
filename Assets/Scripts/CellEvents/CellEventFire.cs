using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellEventFire : CellEvent
{
    private float time;
    private float startTime;
    private bool extinguished = false;
    public CellEventFire (GameObject eventGameObject) : base(eventGameObject)
    {

    }
    public override bool isOver ()
    {
        return extinguished;
    }

    public override void Start ()
    {
        time = Random.Range(60, 100);
        eventGameObject.transform.localScale = Vector3.one *  0.1f;
    }

    public override void Update ()
    {
        time -= Time.deltaTime;
         
        //a zero si spegne
        eventGameObject.transform.localScale = Vector3.one * ((startTime - time) / startTime + 0.1f);
        if(time <= 0)
        {
            //todo: al momento spariscono instantaneamente, sistemare POI
            GameObject.Destroy(this.eventGameObject);
            List<Cell> neighbors = cell.GetNeighbors();
            foreach(var n in neighbors)
            {
                GameObject go = GameObject.Instantiate(this.eventGameObject, n.transform);
                n.SetCellEvent(new CellEventFire(go));
            }
        }

    }

    public override void Hit ()
    {

    }

    public override void OnEnd ()
    {
        GameObject.Destroy(this.eventGameObject);
    }
}
