using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string soundName;
	public AudioClip[] clips;
	[Range(0f, 1f)] public float volume = 1f;

	[Range(0f, 1f)] public float pitch = 1f;
	[Range(0f, 1f)] public float pitchRandomizer;

	public bool loop = false;
	public bool sound3D = true;
	public bool isMusic = false;

	public AudioSource Play() {
		GameObject obj = new GameObject("sound");
		AudioSource source = obj.AddComponent<AudioSource>();
		SoundType soundType = obj.AddComponent<SoundType>();

		source.clip = clips[Random.Range(0, clips.Length)];
		source.spatialBlend = (sound3D ? 1 : 0);

		source.volume = (volume * (isMusic ? MAIN.opVolumeMusicMult : MAIN.opVolumeFXmult));
		source.pitch = pitch + Random.Range(-pitchRandomizer, pitchRandomizer);

		source.loop = loop;
		source.Play();

		//if (!loop) Destroy(gameObject, source.clip.length + 0.1f);

		soundType.sound = this;
		soundType.source = source;

		return source;
	}

}
