using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class OptionsMenu : MonoBehaviour
{
    public Slider vol, FX;


    private void Start() {
        vol.SetValueWithoutNotify(MAIN.opVolumeMusicMult);
        FX.SetValueWithoutNotify(MAIN.opVolumeFXmult);
    }

    public void SetMusicVolume(float volume)
    {
        MAIN.opVolumeMusicMult = volume;
    }

    public void SetSoundVolume(float volume)
    {
        MAIN.opVolumeFXmult = volume;
    }


    
    public void Save() {
        string[] data = {
            MAIN.opVolumeMusicMult.ToString(),
            MAIN.opVolumeFXmult.ToString()
        };
        
        File.WriteAllLines(Application.persistentDataPath + "/settings.txt", data);
    }
}
