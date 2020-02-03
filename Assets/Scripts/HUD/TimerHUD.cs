using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerHUD : MonoBehaviour {
    public Text text;


    void Update() {
        MAIN.timer += Time.deltaTime;

        text.text = MAIN.TimerFormat(MAIN.timer);
    }
}
