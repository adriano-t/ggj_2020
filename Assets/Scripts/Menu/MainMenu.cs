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
     
    private bool ControllerConnected ()
    {
        string[] temp = Input.GetJoystickNames();
        //Check whether array contains anything
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    //Not empty, controller temp[i] is connected
                    return true;
                    //Debug.Log("Controller " + i + " is connected using: " + temp[i]);
                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    //Debug.Log("Controller: " + i + " is disconnected.");
                    return false;

                }
            }
        }
        return false;
    }
    private void Start ()
    {
        if(vol)
            vol.SetValueWithoutNotify(MAIN.opVolumeMusicMult);

        if (FX)
            FX.SetValueWithoutNotify(MAIN.opVolumeFXmult);

        StartCoroutine(RoutineWait());
    }

    IEnumerator RoutineWait ()
    {
        yield return new WaitForSeconds(0.1f);
        if (!ControllerConnected())
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("No controller found");
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
        //todo
        bool dualsticks = toggleSticks.isOn;
    }

    public void Save ()
    {
        string[] data = {
            MAIN.opVolumeMusicMult.ToString(),
            MAIN.opVolumeFXmult.ToString()
        };

        File.WriteAllLines(Application.persistentDataPath + "/settings.txt", data);
    }
    #endregion

    #region GameOver

    public void GoToMainMenu ()
    {
        SceneManager.LoadScene("Menu");
    }
    public void SendHighScore ()
    {
        //
    }

    #endregion
}
