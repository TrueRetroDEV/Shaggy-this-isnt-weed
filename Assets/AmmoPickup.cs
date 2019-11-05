using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : BPickup {

    public WeaponData weaponType;
    public int ammo;

    public override void PickupItem() {
        for (int i = 0; i < Player.Instance.weapons.Length; i++) {
            if (Player.Instance.weapons[i].weaponData.name == weaponType.name) {
                Player.Instance.weapons[i].ammo += ammo;
            }
        }

        base.PickupItem();
    }

}
