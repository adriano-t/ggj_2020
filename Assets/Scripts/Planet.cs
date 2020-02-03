
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Planet : MonoBehaviour
{
	[Header("Initial Generation")]
	public int seed;
	[Range(0f, 1f)] public float density;

	[Header("Planet Settings")]
	public float radius = 10;
	public Transform navicella;
	public Cell[] cells;


	private void Awake() {
		if (seed < 0) seed = Random.Range(100, 9000000);
	}

	public float CalculateCo2 ()
	{
		float val = 0;
		foreach(Cell cell in cells)
		{
			val += cell.GetCo2Contribution();
		}

		return val;
	}

	public void GenerateSurface() {
		GlobalController global = MAIN.GetGlobal();

		int occupiedCells = (int)(cells.Length * density);
		List<Cell> cellList = new List<Cell>(cells);

		MAIN.Shuffle(cellList, seed);

		while (occupiedCells > 0 && cellList.Count > 0)
		{
			Cell c = cellList[0];

			// se c'è roba solid posizionata, skippa
			if (Physics.OverlapSphere(c.transform.position, 1, 1 << 10).Length > 0) continue;

			if (Random.Range(0, 1) == 0)
			{
				c.SetStato(MAIN.Choose(Cell.Stato.piante, Cell.Stato.semi,
					Cell.Stato.deserto, Cell.Stato.deserto,
					Cell.Stato.ghiaccio)
					);
			}
			else
			{
				if (Random.value > 0.5f)
					c.SetFire();
			}



			occupiedCells--;
			cellList.RemoveAt(0);
		}

		MAIN.Shuffle(cells, seed);
	}


	public float GetRadius() {
		return radius;
	}
	public Vector3 GetCenter() {
		return transform.position;
	}
}