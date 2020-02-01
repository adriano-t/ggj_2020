using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Planet : MonoBehaviour
{
	[SerializeField]
	private GameObject hexagon = null;
	
	void Start()
	{
		double posX = 0;
		double posY = 0;
		double posZ = 0;

		double dist = GetRadius();

		for (double i = 0; i < 2*Math.PI; i += Math.PI/5)
		{
			//for (double j = 0; j < 2*Math.PI; j += Math.PI/5)
			{
				posX = this.transform.position.x + Math.Cos(i) * dist;
				posY = this.transform.position.z + Math.Sin(i) * dist;
				posY = 

				Debug.Log(posX + ", " + posY);
				
				GameObject obj = Instantiate(hexagon, null);
				obj.transform.position = new Vector3((float) posX, (float) posY, 0);
				// obj.transform.rotation = new Q
				transform.rotation = Quaternion.Euler(MAIN.GetDir(GetCenter(), obj.transform.position));
			}
		}
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