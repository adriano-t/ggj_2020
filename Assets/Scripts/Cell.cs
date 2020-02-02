using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour {
	private float temperature;
	private float waterLevel;
	private List<Cell> neighbors = new List<Cell>();
	private CellEvent currentEvent;

	Material mat;
	Planet planet;

	public float Temperature {
		get { return this.temperature; }
		set { this.temperature = Mathf.Min(0, Mathf.Max(this.temperature + value, 100)); }
	}

	public float WaterLevel {
		get { return this.waterLevel; }
		set { this.waterLevel = Mathf.Min(0, Mathf.Max(this.waterLevel + value, 100)); }
	}

	void Start() {
		//GameObject obj = new GameObject("coll");
		//obj.transform.position = transform.position;
		var c = gameObject.AddComponent<MeshCollider>();
		c.convex = true;

		//CapsuleCollider c = obj.AddComponent<CapsuleCollider>();
		//c.radius = 3.25f;
		//c.height = 15;
		//c.isTrigger = true;
		//MAIN.Orient(obj.transform, 7.5f);
		//obj.transform.SetParent(transform);

		planet = MAIN.GetGlobal().GetActivePlanet();
		Renderer rend = GetComponent<MeshRenderer>();
		mat = new Material(rend.material);

		mat.SetColor("_Color", mat.GetColor("_Color") * (Array.IndexOf(planet.cells, this) % 2 == 0 ? 0.9f : 1.1f));
		rend.material = mat;



		Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
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

	private void OnDestroy ()
	{
		if (mat) Destroy(mat);
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
		Debug.Log("asd", gameObject);
		// cambiare materiale cella (array MAIN.GetGlobal().materials.xxx)
		switch (weaponIndex)
		{
			default:
				break;
		}
	}
	 

}
