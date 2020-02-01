using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{ 
    public struct StructWeapon
    {
        public ParticleSystem particles;
        public GameObject obj;
    }
    public StructWeapon[] weapons;
    public int selectedWeapon = 0;
    public float weaponRange = 2.0f;
    void Start()
    {
        RefreshWeapon();
    }

    private void RefreshWeapon ()
    {
        //deactivate all the weapons except the selected
        for (int i = 0; i < weapons.Length; i++)
            if (i != selectedWeapon)
                weapons[i].obj.SetActive(false);
    }
    
    public void GetNextWeapon()
    { 
        selectedWeapon = (selectedWeapon + 1) % weapons.Length;
        RefreshWeapon();
    }

    public void GetPreviousWeapon()
    {
        selectedWeapon--;
        if(selectedWeapon < 0)
            selectedWeapon = weapons.Length - 1;

        RefreshWeapon();
    }

    public void Shoot ()
    {
        weapons[selectedWeapon].particles.Play();

        Ray rayf = new Ray(transform.position, MAIN.GetPlayer().transform.forward);
        Ray rayn = new Ray(MAIN.GetGlobal().GetActivePlanet().GetCenter(), rayf.GetPoint(weaponRange));

        RaycastHit[] hits = Physics.RaycastAll(rayn);
        foreach (RaycastHit hit in hits)
        {
            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell)
            {
                
            }
        }

        //var hits = Physics.OverlapSphere(transform.position + transform.forward, 2);
        
        Debug.DrawLine(transform.position, transform.position + transform.forward * weaponRange, Color.red, 10.0f);
    }
}
