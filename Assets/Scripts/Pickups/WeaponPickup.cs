using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : BPickup {

    public WeaponData weapon;

    public int weaponAmmo;

    public override void PickupItem() {
        Player.Instance.PickupWeapon(weapon, weaponAmmo);

        base.PickupItem();
    }

}
