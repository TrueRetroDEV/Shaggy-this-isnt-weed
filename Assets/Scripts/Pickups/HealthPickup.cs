using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : BPickup {

    public int health;

    public override void PickupItem() {
        Player.Instance.AddHealth(health);

        base.PickupItem();
    }

}
