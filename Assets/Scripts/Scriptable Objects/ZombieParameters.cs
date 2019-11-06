using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieParameters", menuName = "Zombie Parameters", order = 1)]
public class ZombieParameters : ScriptableObject {

    private static ZombieParameters instance;
    public static ZombieParameters Instance {
        get {
            if (!instance) {
                instance = (ZombieParameters)Resources.Load("ZombieParameters");
            }

            return instance;
        }
    }

    public GameObject zombiePrefab;

}
