using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemParameters", menuName = "Item Parameters", order = 1)]
public class ItemParameters : ScriptableObject {

    private static ItemParameters instance;
    public static ItemParameters Instance {
        get {
            if (!instance) {
                instance = (ItemParameters)Resources.Load("ItemParameters");
            }

            return instance;
        }
    }

    public GameObject weaponPickup;
    public GameObject healthPickup;
    public GameObject ammoPickup;

}
