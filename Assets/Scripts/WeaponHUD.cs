using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponHUD
{
    [SerializeField] public Image[] weaponHUDComponents;
    [SerializeField] public WeaponHUDImage[] weaponImages;

    public void SetWeapon(int currentWeaponSelectedWeapon)
    {
        this.ClearImages();
        this.weaponHUDComponents[currentWeaponSelectedWeapon].sprite =  this.weaponImages[currentWeaponSelectedWeapon].image;
    }

    public void ClearImages()
    {
        for (var i = 0; i < weaponHUDComponents.Length; i++)
        {
            this.weaponHUDComponents[i].sprite =  this.weaponImages[i].imageGrayScale;
        }
    }
}
