using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPickup : MonoBehaviour {

    public float range = 1;

    float delay = 0.5f;
    float holdTime = 0.0f;

    void Update() {

        if ((Player.Instance.transform.position - transform.position).sqrMagnitude < (range * range)) {
            if (Input.GetKeyUp(KeyCode.E)) {
                holdTime = 0;
            }
            else if (Input.GetKey(KeyCode.E)) {
                holdTime += Time.deltaTime;

                if (holdTime >= delay) {
                    PickupItem();
                }
            }

        }
        else {
            holdTime = 0;
        }
    }

    public virtual void PickupItem() {
        DestroyItem();
    }

    void DestroyItem() {
        Destroy(gameObject);
    }
}
