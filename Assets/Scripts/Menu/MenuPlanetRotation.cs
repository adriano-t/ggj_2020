using UnityEngine;
using System.Collections;

public class MenuPlanetRotation : MonoBehaviour
{

    public float rotationSpeed = 15.0f; // Degrees per second

    void Update()
    {
        //transform.rotation = new Quaternion();
        transform.Rotate(transform.up, rotationSpeed * Time.deltaTime);
    }

}