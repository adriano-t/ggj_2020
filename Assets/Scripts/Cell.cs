using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour
{
    private float temperature;
    private float waterLevel;

    private CellEvent currentEvent;

    public float Temperature
    {
        get { return this.temperature; }
        set { this.temperature = Mathf.Min(0, Mathf.Max(this.temperature + value, 100)); }
    }
    
    public float WaterLevel
    {
        get { return this.waterLevel; }
        set { this.waterLevel = Mathf.Min(0, Mathf.Max(this.waterLevel + value, 100)); }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        if (this.currentEvent != null)
        {
            this.currentEvent.Update();

            if (this.currentEvent.isOver())
            {
                this.currentEvent == null;
            }
        }
    }

    public void setCellEvent(CellEvent cellEvent)
    {
        this.currentEvent = cellEvent;
        this.currentEvent.Start();
    }

}
