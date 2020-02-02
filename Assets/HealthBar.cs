using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image[] hudCells;

    public void SetValue (float atmosphereValue)
    { 
        int val = Mathf.RoundToInt(hudCells.Length * Mathf.Clamp01(atmosphereValue));
        for (int i = 0; i < hudCells.Length; i++)
        {
            hudCells[i].transform.GetChild(0).gameObject.SetActive(i < val);
        }


    }
}
