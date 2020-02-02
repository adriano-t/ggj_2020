using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{ 
	[System.Serializable]
    public struct StructWeapon
    {
        public ParticleSystem particles;
        public GameObject obj;
        public GameObject bullet;
        public GameObject explosion;
        public Sound[] sounds;
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
        StructWeapon w = weapons[selectedWeapon];
        Planet p = MAIN.GetGlobal().GetActivePlanet();
        w.particles.Play();

        Ray rayf = new Ray(transform.position, MAIN.GetPlayer().transform.forward);
        Ray rayn = new Ray(p.GetCenter(), rayf.GetPoint(weaponRange));
        
        RaycastHit[] hits = Physics.RaycastAll(rayn);
        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.transform)
                Debug.LogError("LEZZO", gameObject);
            Cell cell = hit.collider.transform.GetComponent<Cell>();
            if (cell)
            { 
                cell.Hit(selectedWeapon);
                GameObject bullet = Instantiate(w.bullet, transform.position, transform.rotation);
                StartCoroutine(Trajectory(w, bullet.transform, rayn.GetPoint(p.GetRadius() + MAIN.GetPlayer().height * 0.5f), cell.transform.position));
            }
        }

        //var hits = Physics.OverlapSphere(transform.position + transform.forward, 2);
        
        Debug.DrawLine(transform.position, transform.position + transform.forward * weaponRange, Color.red, 10.0f);
    }

    IEnumerator Trajectory(StructWeapon w, Transform bullet, Vector3 mid, Vector3 end)
    { 

        Vector3 startPos = bullet.position;
        for (float i = 0; i < 1; i+= Time.deltaTime) 
        {
            var pos = Vector3.Lerp(startPos, mid, i);
            bullet.position = Vector3.Lerp(pos, end, i); 
            yield return null;
        }

        GameObject explos = Instantiate(w.explosion, end, Quaternion.identity);
        MAIN.Orient(explos.transform);
        Destroy(bullet.gameObject, 0);
    }
}

