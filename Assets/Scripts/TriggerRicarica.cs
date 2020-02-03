using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRicarica : MonoBehaviour
{

    bool entered = true;

    void Update ()
    {
        if (Vector3.Distance(transform.position, MAIN.GetPlayer().transform.position) < 3f)
        {
            if (!entered)
            {
                MAIN.GetPlayer().ammo.Reload();
                MAIN.SoundPlay(MAIN.GetGlobal().sounds, "ricarica", transform.position);

                entered = true;
            }
        }
        else
        {
            entered = false;
        }
    }
}
