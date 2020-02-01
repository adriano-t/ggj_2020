using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Evento che accade in una determinata cella (incendio, ecc)
public abstract class CellEvent
{
    private Cell cell;
    
    public abstract void Start();

    public abstract void Update();

    public abstract bool isOver();
}
