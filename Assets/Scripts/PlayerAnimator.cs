using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	
	public void Step() {
		MAIN.SoundPlay(MAIN.GetGlobal().sounds, "steps", transform.position);
	}

	public void Shoot() {
		MAIN.GetPlayer().Shoot();
		Invoke("ShootAnimEnd", 0.2f);
	}

	public void ShootAnimEnd() {

		MAIN.GetPlayer().anim.SetBool("shoot", false);
	}


}
