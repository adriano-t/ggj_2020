using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheck : MonoBehaviour
{
    public GameObject tutKey, tutPad;

    // Update is called once per frame
    void Update()
    {
        bool j = MAIN.ControllerConnected();

        tutKey.SetActive(!j);
        tutPad.SetActive(j);
    }
}
