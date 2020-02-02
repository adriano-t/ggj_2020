using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour {
	private List<Cell> neighbors = new List<Cell>();

	public enum Stato {
		erba,
		semi,
		piante,
		foresta,
		semifuoco,
		piantefuoco,
		forestafuoco,
		fuoco,
		deserto,
		ghiaccio
	};

	public Stato stato;
	Stato oldStato;
	List<GameObject> prefs = new List<GameObject>();

	Material mat;
	Planet planet;
	GlobalController global;



	void Start() {
		var c = gameObject.AddComponent<MeshCollider>();
		c.convex = true;

		global = MAIN.GetGlobal();


		planet = MAIN.GetGlobal().GetActivePlanet();
		Renderer rend = GetComponent<MeshRenderer>();
		mat = new Material(rend.material);

		mat.SetColor("_Color", mat.GetColor("_Color") * (Array.IndexOf(planet.cells, this) % 2 == 0 ? 0.9f : 1.1f));
		rend.material = mat;

		StartCoroutine(UpdateSlow());

		Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
		foreach (var coll in colliders)
			if (coll != c)
				neighbors.Add(coll.GetComponentInParent<Cell>());

	}


	IEnumerator UpdateSlow()
	{
		while (true)
		{
			if (stato == Stato.forestafuoco)
			{
				yield return new WaitForSeconds(10);
				if (stato == Stato.forestafuoco) stato = Stato.deserto;
			}

			if (stato == Stato.piantefuoco)
			{
				yield return new WaitForSeconds(7);
				if (stato == Stato.piantefuoco) stato = Stato.deserto;
			}

			if (stato == Stato.semifuoco)
			{
				yield return new WaitForSeconds(5);
				if (stato == Stato.semifuoco) stato = Stato.deserto;
			}



			if (stato != oldStato)
			{
				while (prefs.Count > 0)
				{
					Vanish(prefs[0]);
					prefs.RemoveAt(0);
				}
			}

			oldStato = stato;
			yield return new WaitForSeconds(0.5f);
		}
	}

	void Vanish(GameObject obj)
	{
		obj.transform.position = Vector3.up * 100000;
		Destroy(obj, 10);
	}
	private void OnDestroy ()
	{
		if (mat) Destroy(mat);
	}

	internal List<Cell> GetNeighbors ()
	{
		return neighbors;
	}

	void SetMaterial(int index)
	{
		if (mat) GetComponent<MeshRenderer>().material = MAIN.GetGlobal().cellMaterials[index];
	}

	public void Hit(int weaponIndex)
	{
		SetMaterial(weaponIndex);

		switch (weaponIndex)
		{
			case 0: //acqua
				{
					if (stato == Stato.semi) stato = Stato.piante;
					else if (stato == Stato.piante) stato = Stato.foresta;
					else if (stato == Stato.deserto) stato = Stato.erba;
					break;
				}

			case 1: //fuoco
				{
					if (stato == Stato.semi) stato = Stato.semifuoco;
					else if (stato == Stato.piante) stato = Stato.piantefuoco;
					else if (stato == Stato.foresta) stato = Stato.forestafuoco;
					else if (stato == Stato.ghiaccio) stato = Stato.erba;
					else if (stato == Stato.erba) stato = Stato.deserto;

					if (!prefs.Contains(global.incendio)) InstantiateObj(global.incendio);
					break;
				}

			case 2: //semi
				{
					if (stato == Stato.erba) stato = Stato.semi;
					
					break;
				}

			case 3: //vento
				{

					break;
				}

			default:
				break;
		}
	}
	 
	void InstantiateObj(GameObject obj)
	{
		GameObject o = Instantiate(obj, transform.position, Quaternion.identity);
		MAIN.Orient(o.transform);

		prefs.Add(o);
	}

	public void SetStato(Stato s)
	{
		if (!global) global = MAIN.GetGlobal();
		stato = s;

		switch (stato)
		{
			case Stato.erba:
				break;
			case Stato.semi:
				InstantiateObj(global.prefabSemi);
				break;
			case Stato.piante:
				InstantiateObj(global.prefabPiante);
				break;
			case Stato.foresta:
				InstantiateObj(global.prefabForest);
				break;
			case Stato.semifuoco:
				break;
			case Stato.piantefuoco:
				break;
			case Stato.forestafuoco:
				break;
			case Stato.fuoco:
				break;
			case Stato.deserto:
				break;
			case Stato.ghiaccio:
				InstantiateObj(global.iceberg);
				break;
			default:
				break;
		}
	}

}
