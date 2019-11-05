using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : BPickup {

    public WeaponData.AmmoType ammoType;
    public int amount;

    public override void PickupItem() {
        for (int i = 0; i < Player.Instance.weapons.Length; i++) {
            if (Player.Instance.weapons[i].weaponData.ammoType == ammoType) {
                Player.Instance.weapons[i].ammo += amount;
            }
        }

        base.PickupItem();
    }

}
