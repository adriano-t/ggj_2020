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
		deserto,
		desertofuoco,
		ghiaccio
	};

	public Stato stato;
	Stato oldStato;
	List<GameObject> prefs = new List<GameObject>();

	Material mat;
	Planet planet;

	public float GetCo2Contribution ()
	{
		switch (stato)
		{
			case Stato.piante:
				return -1;
			case Stato.foresta:
				return -2;
			case Stato.semifuoco:
				return +2;
			case Stato.piantefuoco:
				return +2;
			case Stato.forestafuoco:
				return +3;
			case Stato.desertofuoco:
				return +2;
			default:
				return 0;
		}

	}

	GlobalController global;



	void Start () {
		var c = gameObject.AddComponent<MeshCollider>();
		c.convex = true;

		global = MAIN.GetGlobal();


		planet = MAIN.GetGlobal().GetActivePlanet();
		Renderer rend = GetComponent<MeshRenderer>();
		//mat = new Material(rend.material);

		//mat.SetColor("_Color", mat.GetColor("_Color") * (Array.IndexOf(planet.cells, this) % 2 == 0 ? 0.9f : 1.1f));
		//rend.material = mat;

		StartCoroutine(UpdateSlow());

		Collider[] colliders = Physics.OverlapSphere(transform.position, 5, 1 << 11);
		foreach (var coll in colliders)
			if (coll != c)
				neighbors.Add(coll.GetComponentInParent<Cell>());

	}


	IEnumerator UpdateSlow ()
	{
		while (true)
		{
			if (stato == Stato.forestafuoco)
			{
				yield return new WaitForSeconds(10);
				if (stato == Stato.forestafuoco) SetStato(Stato.deserto);
			}

			if (stato == Stato.piantefuoco)
			{
				yield return new WaitForSeconds(7);
				if (stato == Stato.piantefuoco) SetStato(Stato.deserto);
			}

			if (stato == Stato.semifuoco)
			{
				yield return new WaitForSeconds(5);
				if (stato == Stato.semifuoco) SetStato(Stato.deserto);
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

	void Vanish (GameObject obj)
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

	void SetMaterial (int index)
	{
		//if (mat) 
		GetComponent<MeshRenderer>().material = MAIN.GetGlobal().cellMaterials[index];
	}

	public void Hit (int weaponIndex)
	{
		Stato o = stato;

		switch (weaponIndex)
		{
			case 0: //acqua
				{
					if (stato == Stato.semi)
					{
						SetStato(Stato.piante);
					}
					else if (stato == Stato.piante)
					{
						SetStato(Stato.foresta);
					}
					if (stato == Stato.semifuoco)
					{
						SetStato(Stato.semi);
					}
					else if (stato == Stato.piantefuoco)
					{
						SetStato(Stato.piante);
					}
					else if (stato == Stato.forestafuoco)
					{
						SetStato(Stato.foresta);
					}
					else if (stato == Stato.deserto)
					{
						SetStato(Stato.erba);
					}
					else if (stato == Stato.desertofuoco)
					{
						SetStato(Stato.deserto);
					}
					break;
				}

			case 1: //fuoco
				{
					if (stato == Stato.semi)
					{ 
						SetStato(Stato.semifuoco, false);
					}
					else if (stato == Stato.piante)
					{ 
						SetStato(Stato.piantefuoco, false);
					}
					else if (stato == Stato.foresta)
					{ 
						SetStato(Stato.forestafuoco, false); 
					}
					else if (stato == Stato.ghiaccio)
					{ 
						SetStato(Stato.erba);
					}
					else if (stato == Stato.erba)
					{
						SetStato(Stato.desertofuoco); 
					}
					else if (stato == Stato.deserto)
					{
						SetStato(Stato.desertofuoco); 
					}

					break;
				}

			case 2: //semi
				{
					if (stato == Stato.erba)
					{
						SetStato(Stato.semi);
					}

					break;
				}

			case 3: //vento
				{

					break;
				}

			default:
				break;
		}

		if (o != stato) SetMaterial(weaponIndex);
	}

	public void InstantiateObj (GameObject obj)
	{
		GameObject o = Instantiate(obj, transform.position, Quaternion.identity);
		MAIN.Orient(o.transform);

		prefs.Add(o);
	}

	public void SetStato (Stato s, bool destroyCheck)
	{
		if (!global) global = MAIN.GetGlobal();
		stato = s;

		if (destroyCheck && stato != oldStato)
		{
			while (prefs.Count > 0)
			{
				Vanish(prefs[0]);
				prefs.RemoveAt(0);
			}
		}

		switch (stato)
		{
			case Stato.semi:
				SetMaterial(0);
				InstantiateObj(global.prefabSemi);
				break;
			case Stato.piante:
				SetMaterial(0);
				InstantiateObj(global.prefabPiante);
				break;
			case Stato.foresta:
				SetMaterial(0);
				InstantiateObj(global.prefabForest);
				break;
			case Stato.deserto:
				SetMaterial(1);
				break;
			case Stato.ghiaccio:
				SetMaterial(4);
				InstantiateObj(global.iceberg);
				break;
			case Stato.erba:
				SetMaterial(0);
				break;
			case Stato.semifuoco:
				SetMaterial(1);
				InstantiateObj(global.incendio);
				break;
			case Stato.piantefuoco:
				SetMaterial(1);
				InstantiateObj(global.incendio);
				break;
			case Stato.forestafuoco:
				SetMaterial(1);
				InstantiateObj(global.incendio);
				break;
			case Stato.desertofuoco:
				SetMaterial(1);
				InstantiateObj(global.incendio);
				break;
			default:
				break;
		}

		oldStato = stato;
	}
	public void SetStato (Stato s)
	{
		SetStato(s, true);
	}

	public void SetFire ()
	{
		if (oldStato == Stato.semi) SetStato(Stato.semifuoco, false);
		else if (oldStato == Stato.piante) SetStato(Stato.piantefuoco, false);
		else if (oldStato == Stato.foresta) SetStato(Stato.forestafuoco, false);
		else SetStato(Stato.desertofuoco, false);
	}
	public bool IsSuitableForThunderEvent()
	{
		return !Occupied() && (stato == Stato.erba || stato == Stato.semi || stato == Stato.piante || 
			stato == Stato.foresta || stato == Stato.deserto);
	}

	public bool Occupied()
	{
		
		return false;
	}
}
