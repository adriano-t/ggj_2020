using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public GameObject playerObj;
    public int currentLevel = 0;
    public GameObject[] planets;

    Planet activePlanet;


    void Start() {
        // questo player viene duplicato per il respawn in LoadMap
        playerObj.SetActive(false); 

        LoadMap();
    }



    // carica un nuovo livello inizializzandolo
    public void LoadMap() {
        foreach(GameObject o in planets) {
            o.SetActive(false);
        }

        activePlanet = planets[currentLevel].GetComponent<Planet>();
        planets[currentLevel].SetActive(true);

        GameObject player = Instantiate(playerObj, activePlanet.GetCenter() + Vector3.up * activePlanet.GetRadius(), Quaternion.identity);
        player.SetActive(true);
    }


    public Planet GetActivePlanet() {
        return activePlanet;
    }

}
