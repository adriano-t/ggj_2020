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
	public Animator anim;
    public GameObject cameraObj;
	public Weapon weapon;

	float angle = 90;
    Vector3 direction;
	Quaternion rotation = Quaternion.Euler(-90, 0, 0);
    Camera cam;
    GlobalController global;
	CameraController camController;
    Planet planet;


    void Start() {
		
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

		
		if (camController.cameraBehind) {

			Rotation(inputs.x * rotationVelocity);
			Position(0, (Mathf.Abs(inputs.y) < 0.1f ? 0 : Mathf.Sign(inputs.y)) * velocity * Time.deltaTime);

			transform.position = rotation * Vector3.forward * planet.GetRadius();
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation * Quaternion.LookRotation(direction, Vector3.forward), Time.deltaTime * rotationVelocity);

			anim.SetFloat("speed", Mathf.Abs(inputs.y));
		}
		else {
			

			Position(inputs.x * velocity * Time.deltaTime, inputs.y * velocity * Time.deltaTime);

			transform.position = rotation * Vector3.forward * planet.GetRadius();
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation * Quaternion.LookRotation(direction, Vector3.forward), Time.deltaTime * rotationVelocity);

			float horizontalAim = Input.GetAxisRaw("HorizontalAim");
			float verticalAim = Input.GetAxisRaw("VerticalAim");



			if (Mathf.Abs(horizontalAim) >= 0.05 || Mathf.Abs(verticalAim) >= 0.05) {
				float angleY = Mathf.Atan2(horizontalAim, verticalAim) * Mathf.Rad2Deg;
				Quaternion localRotation = transform.GetChild(0).transform.localRotation;
				transform.GetChild(0).transform.localRotation = Quaternion.Lerp(localRotation, Quaternion.Euler(0, angleY + offset, 0), Time.deltaTime * rotationVelocity * 0.5f);
			}

			anim.SetFloat("speed", inputs.sqrMagnitude);
		}
		#endregion

		

        //TODO Implementare cambio arma

        if(Input.GetButtonDown("ChangeWeaponLeft"))
        {
            weapon.GetPreviousWeapon();
        }
        if(Input.GetButtonDown("ChangeWeaponLeft"))
        {
			weapon.GetNextWeapon();
        }

        if(Input.GetAxisRaw("Fire") == 1)
        {
			anim.SetBool("shoot", true);
        }

        if(Input.GetButtonUp("Pause"))
        {
            //TODO Mostrare menù di pausa
        }

		
    }

	public void Shoot() {
		weapon.Shoot();
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
