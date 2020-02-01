using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}




	public float GetRadius() {
		return transform.localScale.x * 0.5f;
	}
	public Vector3 GetCenter() {
		return transform.position;
	}
}