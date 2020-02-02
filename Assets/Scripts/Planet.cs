﻿using System;
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
	public Cell[] cells;



	public void GenerateSurface() {
		GlobalController global = MAIN.GetGlobal();

		int occupiedCells = (int)(cells.Length * density);
		List<Cell> cellList = new List<Cell>(cells);

		MAIN.Shuffle(cellList, seed);

		while (occupiedCells > 0 && cellList.Count > 0) {
			Cell c = cellList[0];

			//GameObject tree = Instantiate(global.prefabForest, c.transform.position, Quaternion.identity);
			c.SetStato(MAIN.Choose(Cell.Stato.piante, Cell.Stato.semi, Cell.Stato.foresta, Cell.Stato.foresta,
				Cell.Stato.deserto,
				Cell.Stato.ghiaccio)
				);
			

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