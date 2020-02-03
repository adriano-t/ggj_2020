using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void BackToGame()
    {
        SceneManager.LoadScene("Map");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
