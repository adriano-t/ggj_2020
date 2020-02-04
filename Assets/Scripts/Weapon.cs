using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour {
    [System.Serializable]
    public struct StructWeapon {
        public ParticleSystem particles;
        public GameObject obj;
        public GameObject bullet;
        public GameObject explosion;
        public Sound[] sounds;
    }

    public StructWeapon[] weapons;
    public int selectedWeapon = 0;
    public float weaponRange = 2.0f;
    void Start() {
        RefreshWeapon();
    }


    private void RefreshWeapon() {
        //deactivate all the weapons except the selected
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].obj.SetActive(i == selectedWeapon);



        MAIN.GetGlobal().weaponHud.SetWeapon(selectedWeapon);
    }

    public void GetNextWeapon() {
        selectedWeapon = (selectedWeapon + 1) % weapons.Length;
        RefreshWeapon();
    }

    public void GetPreviousWeapon() {
        selectedWeapon--;
        if (selectedWeapon < 0)
            selectedWeapon = weapons.Length - 1;

        RefreshWeapon();
    }

    public void Shoot() {
        StructWeapon w = weapons[selectedWeapon];
        Planet p = MAIN.GetGlobal().GetActivePlanet();
        w.particles.Play();

        Ray rayf = new Ray(transform.position, transform.forward);
        Ray rayn = new Ray(p.GetCenter(), rayf.GetPoint(weaponRange + MAIN.GetPlayer().speed / Time.deltaTime * 0.5f));

        RaycastHit[] hits = Physics.RaycastAll(rayn);
        foreach (RaycastHit hit in hits) {
            Transform t = hit.collider.transform;

            Cell cell = t.GetComponent<Cell>();

            while (!cell) {
                t = t.parent;
                if (t == null) break;
                cell = t.GetComponent<Cell>();
            }

            GameObject bullet = Instantiate(w.bullet, w.obj.transform.GetChild(0).position, transform.rotation);
            StartCoroutine(Trajectory(w, cell, bullet.transform, rayn.GetPoint(p.GetRadius() + MAIN.GetPlayer().height), hit.point));
        }
    }

    IEnumerator Trajectory(StructWeapon w, Cell target, Transform bullet, Vector3 mid, Vector3 end) {

        Vector3 startPos = bullet.position;

        for (float i = 0; i < 1; i += Time.deltaTime * 2) {
            var pos = Vector3.Lerp(startPos, mid, i);
            bullet.position = Vector3.Lerp(pos, end, i);

            yield return null;

            if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit, 8, 1 << 13)) {
                Vector3 center = MAIN.GetGlobal().GetActivePlanet().GetCenter();
                target = Physics.RaycastAll(new Ray(center, MAIN.GetDir(center, hit.collider.transform.parent.position)), 1000, 1 << 11)[0].collider.GetComponent<Cell>();
                yield return new WaitForSeconds((1 - i) * 0.5f);
                break;
            }
        }

        if (target) target.Hit(selectedWeapon);

        if (selectedWeapon == 2) {
            MAIN.SoundPlay(MAIN.GetGlobal().sounds, "pianta seme", end);
        }

        if (w.explosion) {
            GameObject explos = Instantiate(w.explosion, end, Quaternion.identity);
            MAIN.Orient(explos.transform);
        }
        Destroy(bullet.gameObject, 0);
    }

}
