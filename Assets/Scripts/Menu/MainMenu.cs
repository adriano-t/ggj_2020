using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider vol, FX;
    public Toggle toggleSticks;
     
    
    private void Start ()
    {
        vol.SetValueWithoutNotify(MAIN.opVolumeMusicMult);
        FX.SetValueWithoutNotify(MAIN.opVolumeFXmult);
        toggleSticks.SetIsOnWithoutNotify(MAIN.opDualStick);

        StartCoroutine(RoutineWait());
        
    }
    IEnumerator RoutineWait ()
    {
        yield return new WaitForSeconds(0.1f);
        if (!MAIN.ControllerConnected())
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.LogError("asd");
        }
         
    }
    #region Main
    public void PlayGame()
    {
        SceneManager.LoadScene("Map");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Options

    public void SetMusicVolume ()
    {
        MAIN.opVolumeMusicMult = vol.value;
    }

    public void SetSoundVolume ()
    {
        MAIN.opVolumeFXmult = FX.value;
    }
     
    public void ToggleSticks()
    {
        MAIN.opDualStick = toggleSticks.isOn;
    }

    public void Save ()
    {
        string[] data = {
            MAIN.opVolumeMusicMult.ToString(),
            MAIN.opVolumeFXmult.ToString(),
            MAIN.opDualStick.ToString()
        };

        File.WriteAllLines(Application.persistentDataPath + "/settings.txt", data);
    }
    #endregion
}
