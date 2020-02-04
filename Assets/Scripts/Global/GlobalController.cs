using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;


public class GlobalController : MonoBehaviour {
	public bool generate = true;

	[Header("Global Prefabs")]
	public GameObject playerObj;
	public Material[] cellMaterials;
	public TextMeshProUGUI[] highscores;
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

		MAIN.timer = 0;
	}

	void Start() {
		LoadOptions();
 
		
		if (highscores.Length == 2) {
			ScorePrint(highscores[0], highscores[1]);
		}

		string sceneName = SceneManager.GetActiveScene().name; 
		string songName;
		switch (sceneName)
		{
			case "Menu":
				songName = "MenuTheme";
				break;
			case "Map":
				songName = "GameTheme";
				break;
			case "GameOver":
				songName = "GameOver";
				break; 
			default:
				songName = "MenuTheme";
				break;
		} 
		MAIN.SoundPlay(sounds, songName, transform.position); 
		
 
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
		if (data.Length > 2) MAIN.opDualStick = bool.Parse(data[2]);

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

	public void ScoreSend(string name, float score) {
		StartCoroutine(ScoreSendRoutine(name, score));
	}
	IEnumerator ScoreSendRoutine(string name, float score) {
		WWWForm form = new WWWForm();

		form.AddField("name", name.Replace("§", "$")); // § carattere speciale di separazione
		form.AddField("score", score.ToString());

		string date = System.DateTime.Now.Ticks.ToString();
		form.AddField("date", date);

		string tohash = name + score.ToString() + date;
		form.AddField("h", MAIN.HmacSha256Digest(tohash, "Ujs8%ap0=a1WslM88tYc%t)a5!!oLgHv"));
		
		UnityWebRequest web = UnityWebRequest.Post("https://atmospgmi.altervista.org/addscore.php", form);
		yield return web.SendWebRequest();

		//Debug.LogError(web.downloadHandler.text);
		web.Dispose();
	}
	public void ScorePrint(TextMeshProUGUI UITextNames, TextMeshProUGUI UITextScores) {
		StartCoroutine(ScoreGetTop10Routine(UITextNames, UITextScores));
	}
	IEnumerator ScoreGetTop10Routine(TextMeshProUGUI UITextNames, TextMeshProUGUI UITextScores) {
		UnityWebRequest web = UnityWebRequest.Get("https://atmospgmi.altervista.org/get_top_10.php");
		yield return web.SendWebRequest();

		string text = web.downloadHandler.text, names = "", scores = "";
		Debug.Log(text);

		/*
		Formato dell output:
		name1§score1§name2§score2§ecc
		*/

		string[] tab = text.Split('§');

		for (int i = 0; i < tab.Length; i += 2) {
			names += tab[i] + "\n";
			scores += tab[i + 1] + "\n";
		}


		if (UITextNames) UITextNames.text = names.TrimEnd('\n');
		if (UITextScores) UITextScores.text = scores.TrimEnd('\n');

		web.Dispose();
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