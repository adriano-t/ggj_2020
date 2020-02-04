using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

static public class MAIN
{
    static public Player player = null;
    static public GlobalController global = null;
	static public float CO2level = 50;
	static public float timer = 0;

	static public float opVolumeMusicMult = 1;
	static public float opVolumeFXmult = 1;
	static public bool opDualStick = false;



    static public Player GetPlayer() {
        if (player == null) player = GameObject.FindWithTag("player").GetComponent<Player>();
        return player;
    }
    
    static public GlobalController GetGlobal() {
        if (global == null) global = GameObject.FindWithTag("global").GetComponent<GlobalController>();
        return global;
    }

	static public string TimerFormat(float time) {
		int t = Mathf.FloorToInt(time);

		int h, m, s;

		h = Mathf.FloorToInt(t / 3600);
		t -= h * 3600;
		m = Mathf.FloorToInt(t / 60);
		t -= m * 60;
		s = t;

		return (h > 0 ? (h < 10 ? "0" + h.ToString() : h.ToString()) + ":" : "") + (m < 10 ? "0" + m.ToString() : m.ToString()) + ":" + (s < 10 ? "0" + s.ToString() : s.ToString());
	}


	static public AudioSource SoundPlay(Sound[] sounds, string soundName, Vector3 position) {
		foreach (Sound s in sounds) {
			if (s.soundName == soundName) {
				AudioSource source = s.Play();
				if (!source.loop) MAIN.GetGlobal().DestroyThis(source.gameObject, source.clip.length + 0.1f);
				return source;
			}
		}
		return null;
	}

	static public Vector3 GetDir(Vector3 from, Vector3 to) {
		return (to - from).normalized;
	}
	static public void Orient (Transform t)
	{
		Orient(t, 0);
	}
	static public void Orient (Transform t, float offset)
	{
		Planet p = global.GetActivePlanet();
		Ray ray = new Ray(p.GetCenter(), MAIN.GetDir(p.GetCenter(), t.position));

		t.up = MAIN.GetDir(p.GetCenter(), t.position);
		t.up += t.up * offset;

		Vector3 position = ray.GetPoint(p.GetRadius() + offset);
		t.position = position;
	}



	static public string HmacSha256Digest(string message, string secret) {
		ASCIIEncoding encoding = new ASCIIEncoding();
		byte[] keyBytes = encoding.GetBytes(secret);
		byte[] messageBytes = encoding.GetBytes(message);
		HMACSHA256 cryptographer = new HMACSHA256(keyBytes);

		byte[] bytes = cryptographer.ComputeHash(messageBytes);

		return System.BitConverter.ToString(bytes).Replace("-", "").ToLower();
	}

	// generics
	static public void Shuffle<T>(IList<T> list, int seed) {
		if (seed > 0) Random.InitState(seed);

		for (var i = 0; i < list.Count; i++)
			Swap(list, i, Random.Range(i, list.Count));
	}
	static public void Swap<T>(IList<T> list, int i, int j) {
		var temp = list[i];
		list[i] = list[j];
		list[j] = temp;
	}
	static public T Choose<T> (params T[] a)
	{
		return a[Random.Range(0, a.Length)];
	}

	static public bool ControllerConnected() {
		string[] temp = Input.GetJoystickNames();
		//Check whether array contains anything
		if (temp.Length > 0) {
			//Iterate over every element
			for (int i = 0; i < temp.Length; ++i) {
				//Check if the string is empty or not
				if (!string.IsNullOrEmpty(temp[i])) {
					//Not empty, controller temp[i] is connected
					return true;
					//Debug.Log("Controller " + i + " is connected using: " + temp[i]);
				}
				else {
					//If it is empty, controller i is disconnected
					//where i indicates the controller number
					//Debug.Log("Controller: " + i + " is disconnected.");
					return false;

				}
			}
		}
		return false;
	}

}
