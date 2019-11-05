using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "New Weapon", order = 1)]
public class WeaponData : ScriptableObject {

    public enum AmmoType {
        Small,
        Medium,
        Shell,
        Fuel,
        Grenade_Frag,
    }

    [Header("ID")]
    public new string name;

    [Header("Damage Settings")]
    public float damage;

    [Header("Firing Parameters")]
    public float rateOfFire;
    public int roundsPerShot;
    public float burstDelay;
    public bool singleShot;
    public bool dualWield;
    public float weaponSpread;

    [Header("Ammunition")]
    public AmmoType ammoType;
    public int defaultAmmo;
    public int clipSize;
    public float reloadTime;

    [Header("Projectile Parameters")]
    public GameObject projectile;
    public ProjectileData projectileData;
}
