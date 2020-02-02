using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponHUD
{
    [SerializeField] public string[] weaponNames;
    [SerializeField] public Text weaponName;
    [SerializeField] public Image[] weaponHUDComponents;
    [SerializeField] public WeaponHUDImage[] weaponImages;
    [SerializeField] public Image[] backgrounds;

    public void SetWeapon(int currentSelectedWeapon)
    {
        this.ClearImages();
        this.weaponHUDComponents[currentSelectedWeapon].sprite =  this.weaponImages[currentSelectedWeapon].image;
        this.SetWeaponName(currentSelectedWeapon);
    }

    public void ClearImages()
    {
        for (var i = 0; i < weaponHUDComponents.Length; i++)
        {
            this.weaponHUDComponents[i].sprite =  this.weaponImages[i].imageGrayScale;
        }
    }

    public void SetAmmo(int currentSelectedWeapon, int ammo)
    {
        float ratio = (float) ammo / 3;

        this.backgrounds[currentSelectedWeapon].fillAmount = ratio;
    }

    public void SetWeaponName(int currentSelectedWeapon)
    {
        this.weaponName.text = this.weaponNames[currentSelectedWeapon];
    }
}
