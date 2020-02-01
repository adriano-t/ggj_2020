using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disaster : MonoBehaviour
{
    public GameObject prefabEvent;
    void Start()
    {
        
    }

    public void HitCell(Cell cell)
    {
        GameObject go = Instantiate(prefabEvent, cell.transform);
        cell.SetCellEvent(new CellEventFire(go));
    }
     
}
