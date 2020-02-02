using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRicarica : MonoBehaviour
{

    void Update ()
    {
        if (Vector3.Distance(transform.position, MAIN.GetPlayer().transform.position) < 3.5f)
        {
            MAIN.GetPlayer().ammo.Reload();
        }
    }
}
