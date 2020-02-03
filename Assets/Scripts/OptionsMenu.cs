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

    public void SetMusicVolume()
    {
        MAIN.opVolumeMusicMult = vol.value;
    }

    public void SetSoundVolume()
    {
        MAIN.opVolumeFXmult = FX.value;
    }


    
    public void Save() {
        string[] data = {
            MAIN.opVolumeMusicMult.ToString(),
            MAIN.opVolumeFXmult.ToString()
        };
        
        File.WriteAllLines(Application.persistentDataPath + "/settings.txt", data);
    }
}
