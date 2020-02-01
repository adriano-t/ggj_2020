using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ParticleSystem particles;
    public GameObject[] weapons;
    public int selectedWeapon = 0;
    public float weaponRange = 2.0f;
    void Start()
    {
        RefreshWeapon();
    }

    private void RefreshWeapon ()
    {
        //deactivate all the weapons except the selected
        for (int i = 0; i < transform.childCount; i++)
            if (i != selectedWeapon)
                transform.GetChild(i).gameObject.SetActive(false);
    }
    
    public void GetNextWeapon()
    {
        if (selectedWeapon + 1 < weapons.Length)
        {
            selectedWeapon += 1;
            RefreshWeapon();
        }
    }

    public void GetPreviousWeapon()
    {
        if (selectedWeapon - 1 >= 0)
        {
            selectedWeapon -= 1;
            RefreshWeapon();
        }
    }

    public void Shoot ()
    {
        particles.Play();
        Physics.OverlapCapsule(transform.position, transform.position + transform.forward * weaponRange, 0.5f);
        Debug.DrawLine(transform.position, transform.position + transform.forward * weaponRange, Color.red, 10.0f);
    }
}
