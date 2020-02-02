﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")] public float height;
    public float velocity;
    public float offset;
    public float rotationVelocity;

	[Header("Prefabs and Objects")]
	public Animator anim;
    public GameObject cameraObj;
	public Weapon weapon;


    private Vector3 prevPos;
    public float speed;
    float angle = 90;
    Vector3 direction;
	Quaternion rotation = Quaternion.Euler(-90, 0, 0);
    Camera cam;
    GlobalController global;
	CameraController camController;
    Planet planet;

    void Start() 
    {
        speed = 0;
        // la creazione del player avviene nel GlobalController
        cam = cameraObj.GetComponent<Camera>();
        global = MAIN.GetGlobal();
        planet = global.GetActivePlanet(); 

		camController = cam.GetComponent<CameraController>();
        cameraObj.transform.SetParent(null);
    }

    void Update() {
		#region Movement/Aim

		Vector2 inputs = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
        bool mov = true;
		
		if (camController.cameraBehind) {

			Rotation(inputs.x * rotationVelocity);
			mov = Position(0, (Mathf.Abs(inputs.y) < 0.1f ? 0 : Mathf.Sign(inputs.y)) * velocity * Time.deltaTime);

			transform.position = rotation * Vector3.forward * planet.GetRadius();
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation * Quaternion.LookRotation(direction, Vector3.forward), Time.deltaTime * rotationVelocity);

			anim.SetFloat("speed", mov ? Mathf.Abs(inputs.y) : 0);
		}
		else {
			

			mov = Position(inputs.x * velocity * Time.deltaTime, inputs.y * velocity * Time.deltaTime);

			transform.position = rotation * Vector3.forward * planet.GetRadius();
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation * Quaternion.LookRotation(direction, Vector3.forward), Time.deltaTime * rotationVelocity);

			float horizontalAim = Input.GetAxisRaw("HorizontalAim");
			float verticalAim = Input.GetAxisRaw("VerticalAim");



			if (Mathf.Abs(horizontalAim) >= 0.05 || Mathf.Abs(verticalAim) >= 0.05) {
				float angleY = Mathf.Atan2(horizontalAim, verticalAim) * Mathf.Rad2Deg;
				Quaternion localRotation = transform.GetChild(0).transform.localRotation;
				transform.GetChild(0).transform.localRotation = Quaternion.Lerp(localRotation, Quaternion.Euler(0, angleY + offset, 0), Time.deltaTime * rotationVelocity * 0.5f);
			}

			anim.SetFloat("speed", mov ? inputs.sqrMagnitude : 0);
		}

        speed = Vector3.Magnitude(transform.position - prevPos);
        if (!mov) speed = 0;
        prevPos = transform.position;
		#endregion

		

        //TODO Implementare cambio arma
        if(Input.GetButtonDown("ChangeWeaponLeft"))
        {
            Debug.LogWarning("LEFT");
            weapon.GetPreviousWeapon(); 
        }

        if (Input.GetButtonDown("ChangeWeaponRight"))
        {
            Debug.LogWarning("RIGHT");
            weapon.GetNextWeapon();

        }

        if(Input.GetAxisRaw("Fire") > 0.5f && !anim.GetBool("shoot"))
        { 
            Debug.Log("fire");
			anim.SetBool("shoot", true);
        }

      /*  if (Input.GetButtonUp("Pause"))
        {
            //TODO Mostrare menù di pausa
        }*/


    }

	public void Shoot() {
		weapon.Shoot();
	}

    bool Position(float x, float y)
    {
        if (Physics.Raycast(new Ray(transform.position, transform.forward * Mathf.Sign(y)), 1f, 1 << 10)) return false;

        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        Quaternion vRot = Quaternion.AngleAxis(y, perpendicular);
        Quaternion hRot = Quaternion.AngleAxis(x, direction);
        rotation *= hRot * vRot;

        return true;
    }

    void Rotation(float amt)
    {
        angle += amt * Mathf.Deg2Rad * Time.deltaTime;
    }

    public Camera GetCamera()
    {
        return cam;
    }
}
