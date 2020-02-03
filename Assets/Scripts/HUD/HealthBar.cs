using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Image[] hudCells;

    float previousAtmosValue = -1;
    float currentAtmosValue = -1;

    float atmos = -1;

    List<Image> animated = new List<Image>();

    private void Start() {
        StartCoroutine(AnimationRoutine());
    }

    public void SetValue(float atmosphereValue) {
        atmos = atmosphereValue;
        if (animated.Count == 0) UpdateValue();
        // l'update avviene quando è finita la AnimationRoutine
    }
    public void UpdateValue() {
        float atmosphereValue = atmos;
        atmos = -1;

        if (previousAtmosValue == -1) {
            previousAtmosValue = atmosphereValue;
            currentAtmosValue = atmosphereValue;
        }

        animated.Clear();
        int val = Mathf.RoundToInt(hudCells.Length * Mathf.Clamp01(atmosphereValue));

        if (atmosphereValue > previousAtmosValue) {
            // atmos sta peggiorando
            int id = Mathf.RoundToInt(Mathf.Clamp(val + 0, 0, hudCells.Length - 1));
            animated.Add(hudCells[id]);
            animated.Add(hudCells[id].transform.GetChild(0).GetComponent<Image>());
        }
        else {
            int id = Mathf.RoundToInt(Mathf.Clamp(val - 1, 0, hudCells.Length - 1));
            animated.Add(hudCells[id]);
        }



        for (int i = 0; i < hudCells.Length; i++) {
            hudCells[i].transform.GetChild(0).gameObject.SetActive(i < val);
        }

        previousAtmosValue = currentAtmosValue;
        currentAtmosValue = atmosphereValue;
    }


    IEnumerator AnimationRoutine() {
        while (animated.Count == 0)
            yield return null;

        while (true) {
            if (atmos != -1) {
                UpdateValue();
                yield return null;
            }

            if (animated.Count == 0) {
                yield return null;
                continue;
            }

            if (currentAtmosValue > previousAtmosValue) {
                // atmos sta peggiorando
                Color c = animated[0].color;

                for (float i = 0; i < 2; i += Time.deltaTime * 5) {
                    foreach (Image m in animated) {
                        m.color = Color.Lerp(c, Color.red, Mathf.PingPong(i, 1));
                    }
                    yield return null;
                }

                foreach (Image m in animated) {
                    m.color = c;
                }
            }
            else {
                Color c = animated[0].color;

                for (float i = 0; i < 2; i += Time.deltaTime * 5) {
                    foreach (Image m in animated) {
                        Color r = m.color;
                        r.a = Mathf.Lerp(c.a, 0.5f, Mathf.PingPong(i, 1));
                        m.color = r;
                    }
                    yield return null;
                }

                foreach (Image m in animated) {
                    m.color = c;
                }
            }

            yield return null;
        }

        
    }

}
