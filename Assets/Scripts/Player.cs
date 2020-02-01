using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    public float height;
    public float velocity;
    public float offset;
    public float rotationVelocity;

    [Header("Prefabs and Objects")]
    public GameObject cameraObj;


    float angle = 90;
    Vector3 direction;
	Quaternion rotation = Quaternion.Euler(-90, 0, 0);

    Camera cam;
    GlobalController global;
    Planet planet;


    void Start() {
        // la creazione del player avviene nel GlobalController
        cam = cameraObj.GetComponent<Camera>();
        global = MAIN.GetGlobal();
        planet = global.GetActivePlanet();

        cameraObj.transform.SetParent(null);
    }

    void Update() {
        Vector2 inputs = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //transform.rotation = Quaternion.Euler(MAIN.GetDir(transform.position, planet.GetCenter()));
        //Quaternion targetRotation = Quaternion.LookRotation(planet.GetCenter() - transform.position);
        direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));

        //Rotation(inputs.x * rotationVelocity);
        Position(inputs.x *velocity * Time.deltaTime, inputs.y * velocity * Time.deltaTime);

        transform.position = rotation * Vector3.forward * planet.GetRadius();
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation * Quaternion.LookRotation(direction, Vector3.forward), Time.deltaTime * rotationVelocity);
        
        float angleY = Mathf.Atan2(Input.GetAxisRaw("HorizontalAim"), Input.GetAxisRaw("VerticalAim")) *Mathf.Rad2Deg;
        
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, angleY+offset, 0);
        
    }

    void Position(float x, float y) {
		Vector2 perpendicular = new Vector2(-direction.y, direction.x);
		Quaternion vRot = Quaternion.AngleAxis(y, perpendicular);
		Quaternion hRot = Quaternion.AngleAxis(x, direction);
		rotation *= hRot * vRot;
    }
    void Rotation(float amt) {
		angle += amt * Mathf.Deg2Rad * Time.deltaTime;
	}


    public Camera GetCamera() {
        return cam;
    }


}
