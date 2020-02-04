using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Slider vol, FX;
    public Toggle toggleSticks;
    public GameObject pauseMenu;
    public Button continueButton;

    public GameWin winSettings;

    bool sent = false;


    [System.Serializable]
    public struct GameWin {
        public TMP_InputField input;
        public Text scoreUI;
    }
     
    
    private void Start ()
    { 
        if(vol)
            vol.SetValueWithoutNotify(MAIN.opVolumeMusicMult);

        if (FX)
            FX.SetValueWithoutNotify(MAIN.opVolumeFXmult);

        if(toggleSticks)
            toggleSticks.SetIsOnWithoutNotify(MAIN.opDualStick);

        if (winSettings.scoreUI)
            winSettings.scoreUI.text = "<color=\"red\">" + Mathf.Floor(MAIN.timer).ToString() + "</color> seconds";
 
        StartCoroutine(RoutineWait());
        if (pauseMenu) StartCoroutine(PauseController());
    }

    IEnumerator PauseController() {
        while (true) {
            if (Input.GetButtonUp("Pause")) {
                if (!pauseMenu.activeInHierarchy) 
                    MAIN.SoundPlay(MAIN.GetGlobal().sounds, "Pause", transform.position);
                pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
                if(continueButton && MAIN.ControllerConnected())
                    continueButton.Select();
            }

            Time.timeScale = (pauseMenu.activeInHierarchy ? 0 : 1);

            yield return null;
        }
    }

    IEnumerator RoutineWait ()
    {
        yield return new WaitForSeconds(0.1f);
        if (!MAIN.ControllerConnected())
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("No controller found");
        }
         
    }

    #region Main
    public void PlayGame()
    {
        MAIN.timer = 0;
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

    #region GameOver

    public void GoToMainMenu ()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SendHighScore ()
    {
        if (sent || !winSettings.input) return;

        if (winSettings.input.text != "")
            MAIN.GetGlobal().ScoreSend(winSettings.input.text, Mathf.Floor(MAIN.timer));

        sent = true;
    }

    #endregion
}
