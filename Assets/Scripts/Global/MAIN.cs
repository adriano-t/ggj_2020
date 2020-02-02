using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MAIN
{
    static public Player player = null;
    static public GlobalController global = null;


    static public Player GetPlayer() {
        if (player == null) player = GameObject.FindWithTag("player").GetComponent<Player>();
        return player;
    }
    
    static public GlobalController GetGlobal() {
        if (global == null) global = GameObject.FindWithTag("global").GetComponent<GlobalController>();
        return global;
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

}
