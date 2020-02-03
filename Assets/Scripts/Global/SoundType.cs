using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundType : MonoBehaviour
{
    public Sound sound;
    public AudioSource source;

    private void Update() {
        if (source.volume != sound.volume * (sound.isMusic ? MAIN.opVolumeMusicMult : MAIN.opVolumeFXmult)) {
            source.volume = sound.volume * (sound.isMusic ? MAIN.opVolumeMusicMult : MAIN.opVolumeFXmult);
        }
    }
}
