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
		Invoke("ShootAnimEndTimer", 0.7f);
	}

	public void ShootAnimEndTimer ()
	{
		MAIN.GetPlayer().anim.SetBool("shoot", false);
	}
	public void ShootAnimEnd() {

		//MAIN.GetPlayer().anim.SetBool("shoot", false);
	}


}
