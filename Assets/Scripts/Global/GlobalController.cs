﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
	[Header("Global Prefabs")]
    public GameObject playerObj;
    public int currentLevel = 0;

	[Header("Planet Prefabs")]
	public GameObject prefabForest;

	[Header("Sounds")]
	public Sound[] sounds;
    

    Planet activePlanet;




    void Start() {
        LoadMap();
    }



    // carica un nuovo livello inizializzandolo
    public void LoadMap() {
		GameObject[] planets = GameObject.FindGameObjectsWithTag("world");

		foreach (GameObject o in planets) {
            o.SetActive(false);
        }

        activePlanet = planets[currentLevel].GetComponent<Planet>();
        activePlanet.gameObject.SetActive(true);
		activePlanet.GenerateSurface();

        GameObject player = Instantiate(playerObj, activePlanet.GetCenter() + Vector3.up * activePlanet.GetRadius(), Quaternion.identity);
        player.SetActive(true);
    }
	
	public void LoadNextLevel() {
		StartCoroutine(LoadNextLevelRoutine());
	}
	IEnumerator LoadNextLevelRoutine() {

		// fade out

		yield return null;

		currentLevel++;
		if (currentLevel >= 3) currentLevel = 0; // (?)
		LoadMap();

		// fade in

	}

	public void DestroyThis(GameObject obj, float delay) {
		Destroy(obj, delay);
	}

    public Planet GetActivePlanet() {
		if (activePlanet == null)
		{
			var go = GameObject.FindWithTag("world");
			if (go == null)
				Debug.LogError("sgocciola");

			activePlanet = go.GetComponent<Planet>();
		}
        return activePlanet;
    }

}
