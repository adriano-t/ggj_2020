using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class MAIN
{
    static public Player player = null;
    static public GlobalController global = null;


    static public Player GetPlayer() {
        if (player == null) player = GameObject.FindWithTag("player").GetComponent<Player>();
        return player;
    }
    static public GlobalController GetGlobal() {
        if (global == null) global = GameObject.FindWithTag("global").GetComponent<GlobalController>();
        return global;
    }





	static public Vector3 GetDir(Vector3 from, Vector3 to) {
		return (to - from).normalized;
	}


}
