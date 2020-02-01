using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Evento che accade in una determinata cella (incendio, ecc)
public abstract class CellEvent
{
    protected Cell cell;
    protected GameObject eventGameObject;
    public CellEvent(GameObject eventGameObject)
    {
        this.eventGameObject = eventGameObject;
    }

    public abstract void Start();

    public abstract void Update();

    public abstract bool isOver ();
    public abstract void Hit ();

    public abstract void OnEnd ();
}
