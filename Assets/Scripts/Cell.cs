using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour {
	private float temperature;
	private float waterLevel;
	private List<Cell> neighbors = new List<Cell>();
	private CellEvent currentEvent;

	public float Temperature {
		get { return this.temperature; }
		set { this.temperature = Mathf.Min(0, Mathf.Max(this.temperature + value, 100)); }
	}

	public float WaterLevel {
		get { return this.waterLevel; }
		set { this.waterLevel = Mathf.Min(0, Mathf.Max(this.waterLevel + value, 100)); }
	}

	void Start() {
		GameObject obj = new GameObject("coll");
		obj.transform.position = transform.position;
		CapsuleCollider c = obj.AddComponent<CapsuleCollider>();
		c.radius = 3;
		c.height = 15;
		c.isTrigger = true;
		MAIN.Orient(obj.transform);
		obj.transform.SetParent(transform);

		Collider[] colliders = Physics.OverlapSphere(transform.position, 2 * c.radius);
		foreach (var coll in colliders)
			if (coll != c)
				neighbors.Add(coll.GetComponentInParent<Cell>());

	}

	void Update() {
		if (this.currentEvent != null) {
			this.currentEvent.Update();

			if (this.currentEvent.isOver()) {
				this.currentEvent.OnEnd();
				this.currentEvent = null;
			}
		}
	}

	internal List<Cell> GetNeighbors ()
	{
		return neighbors;
	}

	public void SetCellEvent(CellEvent cellEvent) {
		this.currentEvent = cellEvent;
		this.currentEvent.Start();
	}

	public void Hit(int weaponIndex)
	{
		switch (weaponIndex)
		{
			default:
				break;
		}
	}
	 

}
