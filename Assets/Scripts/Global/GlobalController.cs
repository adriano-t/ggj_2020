using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using System.IO;

public class GlobalController : MonoBehaviour {
	public bool generate = true;

	[Header("Global Prefabs")]
	public GameObject playerObj;
	public Material[] cellMaterials;
	public HealthBar healtBar;
	public float difficulty = 1;

	[FormerlySerializedAs("Weapon HUD")] public WeaponHUD weaponHud;
	public int currentLevel = 0;

	[Header("Planet Prefabs")]
	public GameObject prefabSemi;
	public GameObject prefabPiante;
	public GameObject prefabForest;
	public GameObject incendio;
	public GameObject iceberg;

	[Header("Sounds")]
	public Sound[] sounds;


	Planet activePlanet;
	Coroutine routine;


	void Awake() {
		if (generate) LoadMap();
	}

	void Start() {
		LoadOptions();
		MAIN.SoundPlay(sounds, "GameTheme", transform.position);
	}
	void LoadOptions() {
		string path = Application.persistentDataPath + "/settings.txt";

		if (!File.Exists(path)) {
			FileStream file = File.Create(path);
			file.Close();
		}

		string[] data = File.ReadAllLines(path);

		if (data.Length > 0) MAIN.opVolumeMusicMult = float.Parse(data[0]);
		if (data.Length > 1) MAIN.opVolumeFXmult = float.Parse(data[1]);

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

		MAIN.CO2level = 50;

		GameObject player = Instantiate(playerObj, activePlanet.navicella.position + activePlanet.navicella.forward * 3 +
			Vector3.forward * 4f,
			Quaternion.identity);

		player.SetActive(true);
		MAIN.Orient(player.transform);


		if (routine != null)
			StopCoroutine(routine);
		routine = StartCoroutine(RoutineUpdateCo2());
	}

	public void LoadNextLevel() {
		StartCoroutine(LoadNextLevelRoutine());
	}

	IEnumerator RoutineUpdateCo2() {
		while (true) {
			float value = GetActivePlanet().CalculateCo2();
			MAIN.CO2level += value * 0.1f;

			if (MAIN.CO2level >= 100) {
				SceneManager.LoadScene("GameOver");
			}
			else if (MAIN.CO2level <= 0) {
				SceneManager.LoadScene("GameWin");
			}

			healtBar.SetValue(MAIN.CO2level / 100f);
			yield return new WaitForSeconds(1);
		}
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
		if (activePlanet == null) {
			var go = GameObject.FindWithTag("world");
			if (go == null)
				Debug.LogError("sgocciola");

			activePlanet = go.GetComponent<Planet>();
		}

		return activePlanet;
	}
	public Cell FindFreeCell() {
		Cell[] cells = activePlanet.cells;
		Cell c = null;
		int tries = 100;

		while (c == null) {
			c = cells[Random.Range(0, cells.Length)];
			if (!c.IsSuitableForThunderEvent()) c = null;

			tries--;
			if (tries < 0) break;
		}

		return c;
	}

}